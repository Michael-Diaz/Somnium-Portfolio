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


public class EmotionDetection : MonoBehaviour
{
    /* PUBLICS */
    public RawImage rawImage;
    public NNModel modelAsset;

    public Slider scaleFactorSlider;
    public Slider minNeighborsSlider;
    public Slider minSizeSlider;

    public Text scaleFactorSliderText1;
    public Text scaleFactorSliderText2;
    public Text minNeighborsSliderText1;
    public Text minNeighborsSliderText2;
    public Text minSizeSliderText1;
    public Text minSizeSliderText2;
    
    public int prediction; // output index
    public float[] preds; // output array


    /* PRIVATES */
    private CascadeClassifier _cascade;
    private WebCamTexture _webcamTexture;
    private Model _runtimeModel;
    private IWorker _engine;

    private double scaleFactor; // default 1.15
    private int minNeighbors; // default 5
    private int minSize; // default Size(15, 15)
    
    private HersheyFonts _hersheyfont;
    private int _baseLine;
    private Mat _frame;
    private OpenCvSharp.Rect[] _rects;
    private static string[] _labelMap = {"neutral", "happiness", "surprise", "sadness", "anger", "disgust", "fear", "contempt"};

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

        // Loading values for Haar cascade
        path = Application.dataPath + @"/Resources/Cascade_Values/CascadeValues.txt";
        string[] lines = File.ReadAllLines(path);

        // Convert string values to actual values
        scaleFactor = Convert.ToDouble(lines[0]); // default = 1.15
        minNeighbors = Convert.ToInt32(lines[1]); // default = 5
        minSize = Convert.ToInt32(lines[2]); // default = 15

        // Set values for the sliders to these newly loaded values
        scaleFactorSlider.value = (float)scaleFactor;
        minNeighborsSlider.value = minNeighbors;
        minSizeSlider.value = minSize;

        // Set text box values
        UpdateScaleFactorSliderTextBox();
        UpdateMinNeighborsSliderTextBox();
        UpdateMinSizeSliderTextBox();

        // Add listeners for when the values change
        scaleFactorSlider.onValueChanged.AddListener(delegate {OnScaleFactorSliderValueChanged();});
        minNeighborsSlider.onValueChanged.AddListener(delegate {OnMinNeighborsSliderValueChanged();});
        minSizeSlider.onValueChanged.AddListener(delegate {OnMinSizeSliderValueChanged();});
        
        // Emotion detection model set-up
        _runtimeModel = ModelLoader.Load(modelAsset);
        _engine = WorkerFactory.CreateWorker(_runtimeModel, WorkerFactory.Device.Auto);

        // Set the font and baseline for drawing the labels
        _hersheyfont = new HersheyFonts();
        _baseLine = new int();
    }

    // Update is called once per frame
    void Update()
    {
        //UnityEngine.Debug.Log(" EmotionModel Update Called ====================================");

        // Tutorial stuff
        rawImage.texture = _webcamTexture;
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

        // Draw out the boxes and labels
        DrawBoundingBoxes(class_ids.ToArray(), confidences.ToArray());

        // Clear clear faces
        foreach (Texture2D face in faces)
            Texture2D.DestroyImmediate(face, true);
    }

    public void OnScaleFactorSliderValueChanged()
    {
        scaleFactor = (float)scaleFactorSlider.value;
        UpdateScaleFactorSliderTextBox();
    }

    public void OnMinNeighborsSliderValueChanged()
    {
        minNeighbors = (int)minNeighborsSlider.value;
        UpdateMinNeighborsSliderTextBox();
    }

    public void OnMinSizeSliderValueChanged()
    {
        minSize = (int)minSizeSlider.value;
        UpdateMinSizeSliderTextBox();
    }

    public void UpdateScaleFactorSliderTextBox()
    {
        scaleFactorSliderText1.text = (Mathf.Round((float)scaleFactor * 100f) / 100f).ToString();
        scaleFactorSliderText2.text = (Mathf.Round((float)scaleFactor * 100f) / 100f).ToString();
    }

    public void UpdateMinNeighborsSliderTextBox()
    {
        minNeighborsSliderText1.text = minNeighbors.ToString();
        minNeighborsSliderText2.text = minNeighbors.ToString();
    }

    public void UpdateMinSizeSliderTextBox()
    {
        minSizeSliderText1.text = minSize.ToString();
        minSizeSliderText2.text = minSize.ToString();
    }

    public void SaveSliderValues()
    {
        // Create string array of the slider values
        string[] lines =
        {
            scaleFactor.ToString(), 
            minNeighbors.ToString(), 
            minSize.ToString()
        };

        // Save the slider values to their save file
        string path = Application.dataPath + @"/Resources/Cascade_Values/CascadeValues.txt";
        using StreamWriter file = new StreamWriter(path);
        foreach (string line in lines)
            file.WriteLine(line);
    }

    public void ResetSliderValues()
    {
        // Reset slider values to their base defaults
        scaleFactorSlider.value = 1.15f;
        minNeighborsSlider.value = 5;
        minSizeSlider.value = 15;

        // Reset internal values
        scaleFactor = 1.15;
        minNeighbors = 5;
        minSize = 15;
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


    private void DrawBoundingBoxes(int[] class_ids, float[] confidences)
    {
        for (int i = 0; i < _rects.Length; i++)
        {
            // Draw the box on the Mat frame
            _frame.Rectangle(_rects[i], new Scalar(250, 0, 0), 2);

            // Get the label and labelRect (and associated top value)
            string label = _labelMap[class_ids[i]] + ": " + confidences[i].ToString("0.000");
            Size labelSize = Cv2.GetTextSize(label, _hersheyfont, 1, 1, out _baseLine);
            OpenCvSharp.Rect labelRect = new OpenCvSharp.Rect(new Point(_rects[i].X, _rects[i].Y-labelSize.Height), labelSize);
            var top = Mathf.Max((float)_rects[i].Y, (float)labelRect.Height);

            // Draw label background (negative thickness fills area)
            _frame.Rectangle(labelRect, new Scalar(250, 0, 0), -1);

            // Draw label
            _frame.PutText(
                label,
                new Point(_rects[i].X, top),
                _hersheyfont,
                1,
                new Scalar(255, 255, 255)
            );
        }

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
