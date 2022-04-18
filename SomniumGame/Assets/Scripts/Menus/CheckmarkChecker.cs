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
        try {
            string cameraName = Convert.ToString((File.ReadAllLines(Application.dataPath + @"/CameraName.txt"))[0]);
            if (cameraName == label.text) {
                checkmark.SetActive(true);
            } else {
                checkmark.SetActive(false);
            }
        } catch {
            checkmark.SetActive(false);
        }
    }
}