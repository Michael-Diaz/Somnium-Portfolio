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
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using UnityEngine.UI;
using Unity.Barracuda;

using OpenCvSharp;
using AmazingAssets.ResizePro;
using TFClassify;


public class BackgroundEmotionDetection : MonoBehaviour
{
    /* PUBLICS */
    public GameObject UniversalWebcam;
    public NNModel modelAsset;
    
    public int prediction; // output index
    public float[] preds; // output array


    /* PRIVATES */
    private int updateChecker = 10;
    private int updateCounter; // Has the model only run every updateChecker number of frames

    private CascadeClassifier _cascade;
    private WebCamTexture _webcamTexture;
    private Model _runtimeModel;
    private IWorker _engine;

    private double scaleFactor; // default 1.15
    private int minNeighbors; // default 5
    private int minSize; // default Size(15, 15)
    
    private Mat _frame;
    private OpenCvSharp.Rect[] _rects;
    private static string[] _labelMap = {"neutral", "happiness", "surprise", "sadness", "anger", "disgust", "fear", "contempt"};

    // Start is called before the first frame update
    void Start()
    {
        // Set the webcam texture
        _webcamTexture = UniversalWebcam.GetComponent<UniversalWebcam>().webcamTexture;

        // Haar cascade set-up
        string path = Application.dataPath + @"/Resources/Haar_Cascades/haarcascade_frontalface_default.xml";
        _cascade = new CascadeClassifier(path);

        // Loading values for Haar cascade
        path = Application.dataPath + @"/Resources/Cascade_Values/CascadeValues.txt";
        string[] lines = File.ReadAllLines(path);

        // Convert string values to actual values
        scaleFactor = Convert.ToDouble(lines[0]); // default = 1.15
        minNeighbors = Convert.ToInt32(lines[1]); // default = 5
        minSize = Convert.ToInt32(lines[2]); // default = 15
        
        // Emotion detection model set-up
        _runtimeModel = ModelLoader.Load(modelAsset);
        _engine = WorkerFactory.CreateWorker(_runtimeModel, WorkerFactory.Device.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        // UnityEngine.Debug.Log(" EmotionModel Update Called ====================================");
        if (updateCounter <= 0)
        {
            // Set the webcam texture
            _webcamTexture = UniversalWebcam.GetComponent<UniversalWebcam>().webcamTexture;

            // Get the Mat frame
            _frame = OpenCvSharp.Unity.TextureToMat(_webcamTexture);

            // Load faces
            Texture2D[] faces = FindFaces();
            //UnityEngine.Debug.Log($"Found {faces.Length} Faces");

            // Class ids and confidences
            List<int> class_ids = new List<int>();
            List<float> confidences = new List<float>();

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

                    // Postprocess preds
                    preds = Softmax(preds);

                    // Add to the confidence and class lists
                    confidences.Add(preds[prediction]);
                    class_ids.Add(prediction);
                }
            }

            // Clear clear faces
            foreach (Texture2D face in faces)
                Texture2D.DestroyImmediate(face, true);
            
            // Reset update counter
            updateCounter = updateChecker;
        }
        else
        {
            updateCounter--;
        }
    }

    // Don't forget to include "using System.Linq;"
    private float[] Softmax(float[] values)
    {
        var maxVal = values.Max();
        var exp = values.Select(v => Math.Exp(v - maxVal));
        var sumExp = exp.Sum();
        return exp.Select(v => (float)(v / sumExp)).ToArray();
    }

    private Texture2D[] FindFaces()
    {
        // Get the face coords
        // scaleFactor = 1.15, minNeighbors = 5, minSize = new Size(15, 15)
        _rects = _cascade.DetectMultiScale(_frame, scaleFactor, minNeighbors, HaarDetectionType.ScaleImage, new Size(minSize, minSize));

        // Create the Texture2D[]
        Texture2D[] all_faces = new Texture2D[_rects.Length];
        
        // Print the faces and show the rectangles
        for (int i = 0; i < _rects.Length; i++)
        {
            all_faces[i] = CropFaces(_rects[i]);
        }

        return all_faces;
    }


    private Texture2D CropFaces(OpenCvSharp.Rect rect)
    {
        // Get the face
        Mat mat_face = new Mat(_frame, rect);

        Texture tex_face = OpenCvSharp.Unity.MatToTexture(mat_face);
        Texture2D face = TextureToTexture2D(tex_face);
        Destroy(tex_face);

        // Resize texture
        return TextureTools.scaled(face, 64, 64, FilterMode.Bilinear);;
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
        // This mess is for the sake of being able to use grayscale input to the model
        face.SetPixels32(TextureTools.FlipXImageMatrix(face.GetPixels32(), 64, 64));
        Color[] pixels = face.GetPixels();
        Destroy(face);

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
