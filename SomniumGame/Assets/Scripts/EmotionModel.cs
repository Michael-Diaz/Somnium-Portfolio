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

public class EmotionModel : MonoBehaviour
{
    public RawImage rawImage;
    public NNModel model;
    public int predIndex; // output

    private WebCamTexture camTexture;

    // Start is called before the first frame update
    void Start()
    {
        // Use cam_devices to allow a person to select their desired camera
        // For debugging purposes, prints available devices to the console
        WebCamDevice[] cam_devices = WebCamTexture.devices;
        for (int i = 0; i < cam_devices.Length; i++)
            Debug.Log($"Webcam available: {cam_devices[i].name}");

        // Assuming the first available WebCam is desired
        camTexture = new WebCamTexture(cam_devices[0].name);
        rawImage.texture = camTexture;
        rawImage.material.mainTexture = camTexture;
        if (camTexture != null)
            Debug.Log($"Streaming [{cam_devices[0].name}]");
            camTexture.Play();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(" EmotionModel Update Called ====================================");

        // Create worker for the model
        IWorker worker = model.CreateWorker();

        /*
        // Convert camera input directly into a texture
        Texture2D tx2d  = new Texture2D(camTexture.width, camTexture.height);
        tx2d.SetPixels(camTexture.GetPixels());
        tx2d.Apply();
        */
        
        // Crop image to fit input
        Color[] pixels = camTexture.GetPixels();
        int size = 64;
        Texture2D tx2d  = new Texture2D(size, size);
        tx2d.SetPixels(pixels);
        tx2d.Apply();

        // Create input tensor from tx2d
        int channelCount = 1; // you can treat input pixels as 1 (grayscale), 3 (color) or 4 (color with alpha) channels
        Tensor inputTensor = new Tensor(tx2d, channelCount);

        // Input the tensor and get the output
        Tensor outputPreds = worker.Execute(inputTensor).PeekOutput();

        // Access values of the output tensor causing the main thread to block until neural network execution is done
        predIndex = outputPreds.ArgMax()[0];

        // neutral=0, happiness=1, surprise=2, sadness=3, anger=4, disgust=5, fear=6, contempt=7
        
        if (predIndex==0)
            Debug.Log($"Predicted Index: {predIndex} (neutral)");
        else if (predIndex==1)
            Debug.Log($"Predicted Index: {predIndex} (happiness)");
        else if (predIndex==2)
            Debug.Log($"Predicted Index: {predIndex} (surprise)");
        else if (predIndex==3)
            Debug.Log($"Predicted Index: {predIndex} (sadness)");
        else if (predIndex==4)
            Debug.Log($"Predicted Index: {predIndex} (anger)");
        else if (predIndex==5)
            Debug.Log($"Predicted Index: {predIndex} (disgust)");
        else if (predIndex==6)
            Debug.Log($"Predicted Index: {predIndex} (fear)");
        else if (predIndex==7)
            Debug.Log($"Predicted Index: {predIndex} (contempt)");

        Debug.Log($"Prediction Confidence: {outputPreds[predIndex]}");

        // Disposes
        try {
            worker.Dispose();
        } catch (NullReferenceException e) {
            Debug.Log("Failed to dispose of worker IWorker (does worker exist?).");
        }
        try {
            Destroy(tx2d);
        } catch (NullReferenceException e) {
            Debug.Log("Failed to dispose of tx2d Texture2D (does tx2d exist?).");
        }
        try {
            inputTensor.Dispose();
        } catch (NullReferenceException e) {
            Debug.Log("Failed to dispose of inputTensor Tensor (does inputTensor exist?).");
        }
        try {
            outputPreds.Dispose();
        } catch (NullReferenceException e) {
            Debug.Log("Failed to dispose of outputPreds Tensor (does outputPreds exist?).");
        }
    }
}
