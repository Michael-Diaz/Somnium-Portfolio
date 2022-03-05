/*
A script that takes input from the webcam (or any attached camera)
and feeds it into the emotion recognition model.

Note: Need to add the option to select your desired camera device.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using Unity.Barracuda;

using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;


public class EmotionDetection : MonoBehaviour
{
    public RawImage rawImage;
    public NNModel modelAsset;
    public int prediction; // output index
    public float[] preds; // output array

    private CascadeClassifier cascade;
    private MatOfRect faces;
    private WebCamTextureToMatHelper webCamTextureToMatHelper;

    private WebCamTexture _webcamTexture;
    private Model _runtimeModel;
    private IWorker _engine;

    // Start is called before the first frame update
    void Start()
    {
        // Use cam_devices to allow a person to select their desired camera
        // For debugging purposes, prints available devices to the console
        WebCamDevice[] cam_devices = WebCamTexture.devices;
        for (int i = 0; i < cam_devices.Length; i++)
            UnityEngine.Debug.Log($"Webcam available: {cam_devices[i].name}");

        // Assuming the first available WebCam is desired
        _webcamTexture = new WebCamTexture(cam_devices[0].name);
        rawImage.texture = _webcamTexture;
        rawImage.material.mainTexture = _webcamTexture;

        if (_webcamTexture != null) {
            UnityEngine.Debug.Log($"Streaming [{cam_devices[0].name}]");
            _webcamTexture.Play();
        }

        // Haar cascade set-up
        string path = Application.dataPath + @"/Resources/Haar_Cascades/haarcascade_frontalface_default.xml";
        cascade = new CascadeClassifier();
        cascade.load(path);

        // Avoids the front camera low light issue that occurs in only some Android devices (e.g. Google Pixel, Pixel2).
        webCamTextureToMatHelper.avoidAndroidFrontCameraLowLightIssue = true;
        webCamTextureToMatHelper.Initialize();
        
        // Emotion detection model set-up
        _runtimeModel = ModelLoader.Load(modelAsset);
        _engine = WorkerFactory.CreateWorker(_runtimeModel, WorkerFactory.Device.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.Debug.Log(" EmotionModel Update Called ====================================");

        // Get the Mat from the current webcam frame
        Mat rgbaMat = new Mat(_webcamTexture.height, _webcamTexture.width, CvType.CV_8UC5); // empty 0-255, rgba mat
        Mat grayMat = new Mat(_webcamTexture.height, _webcamTexture.width, CvType.CV_8UC1); // empty 0-255, grayscale mat
        Utils.webCamTextureToMat(_webcamTexture, grayMat);

        // Fill faces. Also, resize grayMat for superior performance.
        cascade.detectMultiScale(grayMat, faces, 1.1, 2, 2, new Size(grayMat.cols() * 0.2, grayMat.rows() * 0.2), new Size());

        OpenCVForUnity.CoreModule.Rect[] rects = faces.toArray();
        for (int i = 0; i < rects.Length; i++)
            Imgproc.rectangle(rgbaMat, new Point(rects[i].x, rects[i].y), new Point(rects[i].x + rects[i].width, rects[i].y + rects[i].height), new Scalar(255, 0, 0, 255), 2);
        
        Texture2D texture = new Texture2D(_webcamTexture.height, _webcamTexture.width);
        Utils.matToTexture2D(rgbaMat, texture);









        // Save the current camera frame as a .png
        string path = Application.dataPath + @"/Resources/Model_Images/Cur_Frame/frame.jpg";
        SaveImage(path);
        UnityEngine.Debug.Log("Saved Frame");

        // Find faces in the frame
        RunFile();

        // Load faces
        Texture2D[] faces = Resources.LoadAll<Texture2D>("Model_Images/Cur_Faces/");
        UnityEngine.Debug.Log($"Loaded {faces.Length} Faces");

        if (faces.Length == 0)
            prediction = -1;
        else
        {
            // Loop for detection emotions on detected faces
            for (int i = 0; i < faces.Length; i++) {
                // Create input tensor from tx2d
                int channelCount = 1; // you can treat input pixels as 1 (grayscale), 3 (color) or 4 (color with alpha) channels
                Tensor inputX = new Tensor(faces[i], channelCount);

                // Input the tensor and get the output
                Tensor outPreds = _engine.Execute(inputX).PeekOutput();
                inputX.Dispose();

                // Set public vars
                prediction = outPreds.ArgMax()[0];
                preds = outPreds.AsFloats();
                outPreds.Dispose();

                // neutral=0, happiness=1, surprise=2, sadness=3, anger=4, disgust=5, fear=6, contempt=7
                if (prediction==0)
                    UnityEngine.Debug.Log($"Predicted Index: {prediction} (neutral)");
                else if (prediction==1)
                    UnityEngine.Debug.Log($"Predicted Index: {prediction} (happiness)");
                else if (prediction==2)
                    UnityEngine.Debug.Log($"Predicted Index: {prediction} (surprise)");
                else if (prediction==3)
                    UnityEngine.Debug.Log($"Predicted Index: {prediction} (sadness)");
                else if (prediction==4)
                    UnityEngine.Debug.Log($"Predicted Index: {prediction} (anger)");
                else if (prediction==5)
                    UnityEngine.Debug.Log($"Predicted Index: {prediction} (disgust)");
                else if (prediction==6)
                    UnityEngine.Debug.Log($"Predicted Index: {prediction} (fear)");
                else if (prediction==7)
                    UnityEngine.Debug.Log($"Predicted Index: {prediction} (contempt)");
                UnityEngine.Debug.Log($"Prediction Confidence: {preds[prediction]}");
            }
        }

        // Clear the face(s) in the Cur_Faces folder
        ClearCur_Faces();
        foreach (Texture2D face in faces)
            Texture2D.DestroyImmediate(face, true);
    }

    private static void RunFile()
    {
        Process Proc = new Process();
        string path = Application.dataPath + @"/Resources/Emotion_Detector/dist/";
        Proc.StartInfo.FileName = path + "imgproc.exe";
        Proc.StartInfo.WorkingDirectory = path;
        Proc.Start();
    }

    public Texture2D GetTexture2DFromWebcamTexture(WebCamTexture webCamTexture)
    {
        // Create new texture2d
        Texture2D tx2d = new Texture2D(_webcamTexture.width, _webcamTexture.height);
        // Gets all color data from web cam texture and then Sets that color data in texture2d
        tx2d.SetPixels(webCamTexture.GetPixels());
        // Applying new changes to texture2d
        tx2d.Apply();
        return tx2d;
    }


    public Tensor MakeInputTensor(Texture2D frame)
    {
        // (0, 255) pixel values
        Color32[] pixels = frame.GetPixels32();

        // (0, 1) pixel values
        //Color[] pix = frame.GetPixels();

        // Create empty float array of the needed length
        int inputHeight = 640;
        int inputWidth = 640;
        int numChannels = 3;
        float[] floats = new float[inputHeight * inputWidth * numChannels];

        // Fill the empty float array with properly scaled pixel values
        for (int i = 0; i < pixels.Length; ++i)
        {
            var color = pixels[i];

            // Value range: (0, 255)
            floats[i * numChannels + 0] = color.r;
            floats[i * numChannels + 1] = color.g;
            floats[i * numChannels + 2] = color.b;

            // Value range: (0, 1)
            //floats[i * numChannels + 0] = (color.r / 255f);
            //floats[i * numChannels + 1] = (color.g / 255f);
            //floats[i * numChannels + 2] = (color.b / 255f);

            // Value range: (-1, 1)
            //floats[i * numChannels + 0] = (color.r - 127) / 127.5f;
            //floats[i * numChannels + 1] = (color.g - 127) / 127.5f;
            //floats[i * numChannels + 2] = (color.b - 127) / 127.5f;

        }

        // Create the input tensor from pixel values
        // Batch, height, width, channels, pixel data
        Tensor inputTensor = new Tensor(1, inputHeight, inputWidth, numChannels, floats);
        return inputTensor;
    }

    private void OnDestroy()
    {
        _engine?.Dispose();
    }
}
