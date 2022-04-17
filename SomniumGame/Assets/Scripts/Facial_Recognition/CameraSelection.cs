/*
A script that takes input from the webcam (or any attached camera)
and feeds it into the emotion recognition model.

Note: Need to add the option to select your desired camera device.
*/

using Convert = System.Convert;
using File = System.IO.File;
using StreamWriter = System.IO.StreamWriter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CameraSelection : MonoBehaviour
{
    /* PUBLICS */
    public Text textBox;
    public GameObject UW;

    /* PRIVATES */
    private WebCamDevice[] _camDevices;
    private int _cameraSelection;

    void Start()
    {
        // Load value for camera to load
        string[] lines = File.ReadAllLines(Application.dataPath + @"/Resources/Camera_Selection/CameraSelection.txt");
        _cameraSelection = Convert.ToInt32(lines[0]); // default = 0

        // Use cam_devices to allow a person to select their desired camera
        _camDevices = WebCamTexture.devices;

        // Get the dropdown itself and clear all existing options
        Dropdown dropdown = transform.GetComponent<Dropdown>();
        dropdown.options.Clear();

        // Add the cameras to the dropdown
        for (int i = 0; i < _camDevices.Length; i++)
        {
            dropdown.options.Add(new Dropdown.OptionData() { text = _camDevices[i].name });
        }

        // Start by calling it so the text is initially displayed
        DropdownItemSelected(dropdown);

        // Listener to check for item being selected
        dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(dropdown); });
    }

    
    private void DropdownItemSelected(Dropdown dropdown)
    {
        // Update text in main display
        int index = dropdown.value;
        textBox.text = dropdown.options[index].text;

        // Update saved value in CameraSelection
        using (StreamWriter file = new StreamWriter(Application.dataPath + @"/Resources/Camera_Selection/CameraSelection.txt")) {
            file.WriteLine(index);
        }

        // Update webcamTexture
        UW.GetComponent<UniversalWebcam>().SwitchCamera();
    }
}