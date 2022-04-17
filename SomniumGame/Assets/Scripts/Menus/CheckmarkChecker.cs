using Convert = System.Convert;
using File = System.IO.File;
using StreamWriter = System.IO.StreamWriter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CheckmarkChecker : MonoBehaviour
{
    public Text label;
    public GameObject checkmark;

    void Update()
    {
        // Check to see if this is the selected camera
        string cameraName = Convert.ToString((File.ReadAllLines(Application.dataPath + @"/Resources/Camera_Selection/CameraName.txt"))[0]);
        if (cameraName == label.text) {
            checkmark.SetActive(true);
            UnityEngine.Debug.Log(label.text);
        } else {
            checkmark.SetActive(false);
            
            UnityEngine.Debug.Log("It no work");
        }
    }
}