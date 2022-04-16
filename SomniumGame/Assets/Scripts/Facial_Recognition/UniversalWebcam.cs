/*
A script that takes input from the webcam (or any attached camera)
and feeds it into the emotion recognition model.

Note: Need to add the option to select your desired camera device.
*/

using Convert = System.Convert;
using File = System.IO.File;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UniversalWebcam : MonoBehaviour
{
    /* PUBLICS */
    public WebCamTexture webcamTexture;

    void Start()
    {
        // Load value for camera element
        string[] lines = File.ReadAllLines(Application.dataPath + @"/Resources/Camera_Selection/CameraSelection.txt");
        int cameraSelection = Convert.ToInt32(lines[0]); // default = 0

        // Select desired camera
        WebCamDevice[] cam_devices = WebCamTexture.devices;
        webcamTexture = new WebCamTexture(cam_devices[cameraSelection].name);

        // Start selected camera
        if (webcamTexture != null) {
            UnityEngine.Debug.Log($"Streaming [{cam_devices[cameraSelection].name}]");
            webcamTexture.Play();
        }
    }

    public void SwitchCamera()
    {
        // Stop current camera
        webcamTexture.Stop();

        // Load value for camera element
        string[] lines = File.ReadAllLines(Application.dataPath + @"/Resources/Camera_Selection/CameraSelection.txt");
        int cameraSelection = Convert.ToInt32(lines[0]); // default = 0

        // Select desired camera
        WebCamDevice[] cam_devices = WebCamTexture.devices;
        webcamTexture = new WebCamTexture(cam_devices[cameraSelection].name);

        // Start selected camera
        if (webcamTexture != null) {
            UnityEngine.Debug.Log($"Streaming [{cam_devices[cameraSelection].name}]");
            webcamTexture.Play();
        }
    }
}