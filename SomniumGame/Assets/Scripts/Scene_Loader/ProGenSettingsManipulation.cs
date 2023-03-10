using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProGenSettingsManipulation : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject PauseMenuCamera;

    public GameObject Level;
    public GameObject Dreamer;
    public GameObject HUD;

    public GameObject PauseMenuCanvasCollection;
    public GameObject BackgroundCanvas;
    public GameObject PauseMenuCanvas;
    public GameObject SettingsCanvas;
    public GameObject AudioCanvas;
    public GameObject FaceDetectionCanvas;


    // Listener
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            // Check to see if the settings menu collection canvas is currently active
            // If it is, then close the settings.
            if (PauseMenuCanvasCollection.activeSelf)
            {
                // Switch cameras
                MainCamera.SetActive(true);
                PauseMenuCamera.SetActive(false);

                // Deactivate canvas objects
                PauseMenuCanvasCollection.SetActive(false);
                BackgroundCanvas.SetActive(false);
                PauseMenuCanvas.SetActive(false);
                SettingsCanvas.SetActive(false);
                AudioCanvas.SetActive(false);
                FaceDetectionCanvas.SetActive(false);

                // Activate level objects
                Level.SetActive(true);
                Dreamer.SetActive(true);
                HUD.SetActive(true);
                Dreamer.transform.GetChild(4).gameObject.SetActive(true);
            }
            // If it isn't, open the settings
            else
            {
                // Switch cameras
                MainCamera.SetActive(false);
                PauseMenuCamera.SetActive(true);

                // Activate canvas objects
                PauseMenuCanvasCollection.SetActive(true);
                BackgroundCanvas.SetActive(true);
                PauseMenuCanvas.SetActive(true);
                SettingsCanvas.SetActive(false);
                AudioCanvas.SetActive(false);
                FaceDetectionCanvas.SetActive(false);

                // Deactivate level objects
                Level.SetActive(false);
                Dreamer.SetActive(false);
                HUD.SetActive(false);
                Dreamer.transform.GetChild(4).gameObject.SetActive(false);
            }
        }
    }

    public void ButtonPress()
    {
        // Check to see if the settings menu collection canvas is currently active
        // If it is, then close the settings.
        if (PauseMenuCanvasCollection.activeSelf)
        {
            // Switch cameras
            MainCamera.SetActive(true);
            PauseMenuCamera.SetActive(false);

            // Deactivate canvas objects
            PauseMenuCanvasCollection.SetActive(false);
            BackgroundCanvas.SetActive(false);
            PauseMenuCanvas.SetActive(false);
            SettingsCanvas.SetActive(false);
            AudioCanvas.SetActive(false);
            FaceDetectionCanvas.SetActive(false);

            // Activate level objects
            Level.SetActive(true);
            Dreamer.SetActive(true);
            HUD.SetActive(true);
            Dreamer.transform.GetChild(4).gameObject.SetActive(true);
        }
        // If it isn't, open the settings
        else
        {
            // Switch cameras
            MainCamera.SetActive(false);
            PauseMenuCamera.SetActive(true);

            // Activate canvas objects
            PauseMenuCanvasCollection.SetActive(true);
            BackgroundCanvas.SetActive(true);
            PauseMenuCanvas.SetActive(true);
            SettingsCanvas.SetActive(false);
            AudioCanvas.SetActive(false);
            FaceDetectionCanvas.SetActive(false);

            // Deactivate level objects
            Level.SetActive(false);
            Dreamer.SetActive(false);
            HUD.SetActive(false);
            Dreamer.transform.GetChild(4).gameObject.SetActive(false);
        }
    }
}
