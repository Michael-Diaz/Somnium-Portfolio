using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelGenSettingsManipulation : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject SettingsCamera;

    public GameObject Level;
    public GameObject Dreamer;
    public GameObject Spawn;
    public GameObject Bounds;

    public GameObject PauseMenuCanvasCollection;
    public GameObject BackgroundCanvas;
    public GameObject PauseMenuCanvas;
    public GameObject SettingsCanvas;
    public GameObject GraphicsCanvas;
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
                SettingsCamera.SetActive(false);

                // Deactivate canvas objects
                PauseMenuCanvasCollection.SetActive(false);
                BackgroundCanvas.SetActive(false);
                PauseMenuCanvas.SetActive(false);
                SettingsCanvas.SetActive(false);
                GraphicsCanvas.SetActive(false);
                AudioCanvas.SetActive(false);
                FaceDetectionCanvas.SetActive(false);

                // Activate level objects
                Level.SetActive(true);
                Dreamer.SetActive(true);
                Spawn.SetActive(true);
                Bounds.SetActive(true);
            }
            // If it isn't, open the settings
            else
            {
                // Switch cameras
                MainCamera.SetActive(false);
                SettingsCamera.SetActive(true);

                // Activate canvas objects
                PauseMenuCanvasCollection.SetActive(true);
                BackgroundCanvas.SetActive(true);
                PauseMenuCanvas.SetActive(true);
                SettingsCanvas.SetActive(false);
                GraphicsCanvas.SetActive(false);
                AudioCanvas.SetActive(false);
                FaceDetectionCanvas.SetActive(false);

                // Deactivate level objects
                Level.SetActive(false);
                Dreamer.SetActive(false);
                Spawn.SetActive(false);
                Bounds.SetActive(false);
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
            SettingsCamera.SetActive(false);

            // Deactivate canvas objects
            PauseMenuCanvasCollection.SetActive(false);
            BackgroundCanvas.SetActive(false);
            PauseMenuCanvas.SetActive(false);
            SettingsCanvas.SetActive(false);
            GraphicsCanvas.SetActive(false);
            AudioCanvas.SetActive(false);
            FaceDetectionCanvas.SetActive(false);

            // Activate level objects
            Level.SetActive(true);
            Dreamer.SetActive(true);
            Spawn.SetActive(true);
            Bounds.SetActive(true);
        }
        // If it isn't, open the settings
        else
        {
            // Switch cameras
            MainCamera.SetActive(false);
            SettingsCamera.SetActive(true);

            // Activate canvas objects
            PauseMenuCanvasCollection.SetActive(true);
            BackgroundCanvas.SetActive(true);
            PauseMenuCanvas.SetActive(true);
            SettingsCanvas.SetActive(false);
            GraphicsCanvas.SetActive(false);
            AudioCanvas.SetActive(false);
            FaceDetectionCanvas.SetActive(false);

            // Deactivate level objects
            Level.SetActive(false);
            Dreamer.SetActive(false);
            Spawn.SetActive(false);
            Bounds.SetActive(false);
        }
    }
}
