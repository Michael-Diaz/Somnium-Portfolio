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
    public NNModel dummyModel;

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

    Texture2D Reshape(Texture2D texture2D, int targetX, int targetY)
    {
        RenderTexture rt=new RenderTexture(targetX, targetY,24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D,rt);
        Texture2D result=new Texture2D(targetX,targetY);
        result.ReadPixels(new Rect(0,0,targetX,targetY),0,0);
        result.Apply();
        return result;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("");
        Debug.Log("");
        Debug.Log("");
        Debug.Log("EmotionModel Update Called");

        // Create worker for the model
        IWorker worker = dummyModel.CreateWorker();

        // Crop the image to 224
        Color[] c = camTexture.GetPixels(0, 0, 224, 224);
        Texture2D inputImage = new Texture2D(224, 224);

        // Fill the input image with the pixels
        inputImage.SetPixels(c);
        inputImage.Apply();

        // Get the input tensor
        Tensor inputTensor = new Tensor(inputImage, 3);

        // Input the tensor and get the output
        Tensor outputPreds = worker.Execute(inputTensor).PeekOutput();

        // Access values of the output tensor causing the main thread to block until neural network execution is done
        int predIndex = outputPreds.ArgMax()[0];
        
        Debug.Log($"Predicted Index: {predIndex}");
        Debug.Log($"Prediction Confidence: {outputPreds[predIndex]}");

        // Disposes
        try {
            worker.Dispose();
        } catch (NullReferenceException e) {
            Debug.Log("Failed to dispose of worker IWorker (does worker exist?).");
        }
        try {
            Destroy(inputImage);
        } catch (NullReferenceException e) {
            Debug.Log("Failed to dispose of inputImage Texture2D (does inputImage exist?).");
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
