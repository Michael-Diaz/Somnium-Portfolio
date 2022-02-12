/* A script that takes input from the webcam (or any attached camera)
 * and feeds it into the emotion recognition model.
 *
 * Note: Need to add the option to select your desired camera device.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using Unity.Barracuda;

public class EmotionDetection : MonoBehaviour
{
    public RawImage rawImage;
    public NNModel modelAsset;
    public int prediction; // output
    public float[] preds;

    private WebCamTexture _camTexture;
    private Model _runtimeModel;
    private IWorker _engine;

    // Start is called before the first frame update
    void Start()
    {
        // Use cam_devices to allow a person to select their desired camera
        // For debugging purposes, prints available devices to the console
        WebCamDevice[] cam_devices = WebCamTexture.devices;
        for (int i = 0; i < cam_devices.Length; i++)
            Debug.Log($"Webcam available: {cam_devices[i].name}");

        // Assuming the first available WebCam is desired
        _camTexture = new WebCamTexture(cam_devices[0].name);
        rawImage.texture = _camTexture;
        rawImage.material.mainTexture = _camTexture;
        if (_camTexture != null)
            Debug.Log($"Streaming [{cam_devices[0].name}]");
            _camTexture.Play();
        
        // Set up the model
        _runtimeModel = ModelLoader.Load(modelAsset);
        _engine = WorkerFactory.CreateWorker(_runtimeModel, WorkerFactory.Device.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(" EmotionModel Update Called ====================================");

        /*
        // Convert camera input directly into a texture
        Texture2D tx2d  = new Texture2D(_camTexture.width, _camTexture.height);
        tx2d.SetPixels(_camTexture.GetPixels());
        tx2d.Apply();
        */
        
        // Crop image to fit input
        Color[] pixels = _camTexture.GetPixels();
        int size = 64;
        Texture2D tx2d  = new Texture2D(size, size);
        tx2d.SetPixels(pixels);
        tx2d.Apply();

        // Create input tensor from tx2d
        int channelCount = 1; // you can treat input pixels as 1 (grayscale), 3 (color) or 4 (color with alpha) channels
        Tensor inputX = new Tensor(tx2d, channelCount);
        Destroy(tx2d);

        // Input the tensor and get the output
        Tensor outPreds = _engine.Execute(inputX).PeekOutput();
        inputX.Dispose();

        // Set public vars
        prediction = outPreds.ArgMax()[0];
        preds = outPreds.AsFloats();
        outPreds.Dispose();

        // neutral=0, happiness=1, surprise=2, sadness=3, anger=4, disgust=5, fear=6, contempt=7
        if (prediction==0)
            Debug.Log($"Predicted Index: {prediction} (neutral)");
        else if (prediction==1)
            Debug.Log($"Predicted Index: {prediction} (happiness)");
        else if (prediction==2)
            Debug.Log($"Predicted Index: {prediction} (surprise)");
        else if (prediction==3)
            Debug.Log($"Predicted Index: {prediction} (sadness)");
        else if (prediction==4)
            Debug.Log($"Predicted Index: {prediction} (anger)");
        else if (prediction==5)
            Debug.Log($"Predicted Index: {prediction} (disgust)");
        else if (prediction==6)
            Debug.Log($"Predicted Index: {prediction} (fear)");
        else if (prediction==7)
            Debug.Log($"Predicted Index: {prediction} (contempt)");
        Debug.Log($"Prediction Confidence: {preds[prediction]}");
    }

    private void OnDestroy()
    {
        _engine?.Dispose();
    }
}
