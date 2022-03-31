/*
A script that takes input from the webcam (or any attached camera)
and feeds it into the emotion recognition model.

Note: Need to add the option to select your desired camera device.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UniversalWebcam : MonoBehaviour
{
    /* PUBLICS */
    public WebCamTexture webcamTexture;

    void Start()
    {
        // Use cam_devices to allow a person to select their desired camera
        // For debugging purposes, prints available devices to the console
        WebCamDevice[] cam_devices = WebCamTexture.devices;
        for (int i = 0; i < cam_devices.Length; i++)
            UnityEngine.Debug.Log($"Webcam available: {cam_devices[i].name}");

        // Assuming the first available WebCam is desired
        webcamTexture = new WebCamTexture(cam_devices[0].name);

        if (webcamTexture != null) {
            UnityEngine.Debug.Log($"Streaming [{cam_devices[0].name}]");
            webcamTexture.Play();
        }
    }
}