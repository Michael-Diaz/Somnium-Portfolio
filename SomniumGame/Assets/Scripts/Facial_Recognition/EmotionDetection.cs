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
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using UnityEngine.UI;
using Unity.Barracuda;

using OpenCvSharp;
using AmazingAssets.ResizePro;


public class EmotionDetection : MonoBehaviour
{
    public RawImage rawImage;
    public NNModel modelAsset;
    public int prediction; // output index
    public float[] preds; // output array

    private CascadeClassifier _cascade;
    private WebCamTexture _webcamTexture;
    private Model _runtimeModel;
    private IWorker _engine;

    private Mat _frame;

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

        if (_webcamTexture != null) {
            UnityEngine.Debug.Log($"Streaming [{cam_devices[0].name}]");
            _webcamTexture.Play();
        }

        // Haar cascade set-up
        string path = Application.dataPath + @"/Resources/Haar_Cascades/haarcascade_frontalface_default.xml";
        _cascade = new CascadeClassifier(path);
        
        // Emotion detection model set-up
        _runtimeModel = ModelLoader.Load(modelAsset);
        _engine = WorkerFactory.CreateWorker(_runtimeModel, WorkerFactory.Device.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.Debug.Log(" EmotionModel Update Called ====================================");

        // Tutorial stuff
        rawImage.texture = _webcamTexture;
        _frame = OpenCvSharp.Unity.TextureToMat(_webcamTexture);

        // Load faces
        Texture2D[] faces = FindFaces();
        UnityEngine.Debug.Log($"Found {faces.Length} Faces");

        // Check to see if any faces were detected
        if (faces.Length == 0)
            prediction = -1;
        else
        {
            // Loop for detection emotions on detected faces
            for (int i = 0; i < faces.Length; i++) {
                // Create input tensor from tx2d
                Tensor inputX = MakeInputTensor(faces[i]);

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

        // Clear clear faces
        foreach (Texture2D face in faces)
            Texture2D.DestroyImmediate(face, true);
    }

    private Texture2D[] FindFaces()
    {
        // Get the face coords
        var rects = _cascade.DetectMultiScale(_frame, 1.15, 5, HaarDetectionType.ScaleImage);

        // Create the Texture2D[]
        Texture2D[] all_faces = new Texture2D[rects.Length];
        
        // Print the faces and show the rectangles
        for (int i = 0; i < rects.Length; i++)
        {
            DrawBoundingBoxes(rects[i]);
            all_faces[i] = CropFaces(rects[i]);
        }

        return all_faces;
    }


    private void DrawBoundingBoxes(OpenCvSharp.Rect rect)
    {
        // Draw the box on the Mat frame
        _frame.Rectangle(rect, new Scalar(250, 0, 0), 2);

        // Convert the Mat frame to a texture and show that texture on the rawImage
        Texture tex = OpenCvSharp.Unity.MatToTexture(_frame);
        rawImage.texture = tex;
    }


    private Texture2D CropFaces(OpenCvSharp.Rect rect)
    {
        // Get the face
        Mat mat_face = new Mat (_frame, rect);
        Texture tex_face = OpenCvSharp.Unity.MatToTexture(mat_face);
        Texture2D face = TextureToTexture2D(tex_face);

        // Resize texture
        face.ResizePro(64, 64, false, false, false);
        return face;
    }

    private Texture2D TextureToTexture2D(Texture tex) 
    {
        // Create empty texture2D
        Texture2D tex2D = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);

        // Create and fill render texture to use as a converter
        RenderTexture rendTex = new RenderTexture(tex.width, tex.height, 32);
        Graphics.Blit(tex, rendTex);

        // Convert render texture into texture2D
        tex2D.ReadPixels(new UnityEngine.Rect(0, 0, rendTex.width, rendTex.height), 0, 0);
        tex2D.Apply();
        
        return tex2D;
    }


    public Tensor MakeInputTensor(Texture2D face)
    {
        // (0, 255) pixel values
        //Color32[] pixels = face.GetPixels32();

        // (0, 1) pixel values
        Color[] pixels = face.GetPixels();

        // Create empty float array of the needed length
        int inputHeight = 64;
        int inputWidth = 64;
        int numChannels = 1;
        float[] floats = new float[inputHeight * inputWidth * numChannels];

        // Fill the empty float array with properly scaled pixel values
        for (int i = 0; i < pixels.Length; ++i)
        {
            // Get the color variable (using var for flexibility between Color32 and Color)
            var color = pixels[i].grayscale * 255;
            // var color = pixels[i];

            // Value range: (0, 255)
            floats[i] = color; // grayscale
            //floats[i * numChannels + 0] = color.r;
            //floats[i * numChannels + 1] = color.g;
            //floats[i * numChannels + 2] = color.b;

            // Value range: (0, 1)
            //floats[i] = (color / 255f); // grayscale
            //floats[i * numChannels + 0] = (color.r / 255f);
            //floats[i * numChannels + 1] = (color.g / 255f);
            //floats[i * numChannels + 2] = (color.b / 255f);

            // Value range: (-1, 1)
            //floats[i] = (color - 127) / 127.5f; // grayscale
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
