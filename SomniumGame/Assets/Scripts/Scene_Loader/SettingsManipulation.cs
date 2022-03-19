using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SettingsManipulation : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject SettingsCamera;

    public GameObject Level;
    public GameObject Dreamer;
    public GameObject Spawn;
    public GameObject Bounds;

    public GameObject SettingsMenuCanvasCollection;
    public GameObject BackgroundCanvas;
    public GameObject MainSettingsCanvas;
    public GameObject GraphicsCanvas;
    public GameObject AudioCanvas;


    // Listener
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            // Check to see if the settings menu collection canvas is currently active
            // If it is, then close the settings.
            if (SettingsMenuCanvasCollection.activeSelf)
            {
                MainCamera.SetActive(true);
                SettingsCamera.SetActive(false);

                SettingsMenuCanvasCollection.SetActive(false);
                BackgroundCanvas.SetActive(false);
                MainSettingsCanvas.SetActive(false);
                GraphicsCanvas.SetActive(false);
                AudioCanvas.SetActive(false);

                Level.SetActive(true);
                Dreamer.SetActive(true);
                Spawn.SetActive(true);
                Bounds.SetActive(true);
            }
            // If it isn't, open the settings
            else
            {
                MainCamera.SetActive(false);
                SettingsCamera.SetActive(true);

                SettingsMenuCanvasCollection.SetActive(true);
                BackgroundCanvas.SetActive(true);
                MainSettingsCanvas.SetActive(true);
                GraphicsCanvas.SetActive(false);
                AudioCanvas.SetActive(false);

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
        if (SettingsMenuCanvasCollection.activeSelf)
        {
            MainCamera.SetActive(true);
            SettingsCamera.SetActive(false);
            
            SettingsMenuCanvasCollection.SetActive(false);
            BackgroundCanvas.SetActive(false);
            MainSettingsCanvas.SetActive(false);
            GraphicsCanvas.SetActive(false);
            AudioCanvas.SetActive(false);

            Level.SetActive(true);
            Dreamer.SetActive(true);
            Spawn.SetActive(true);
            Bounds.SetActive(true);
        }
        // If it isn't, open the settings
        else
        {
            MainCamera.SetActive(false);
            SettingsCamera.SetActive(true);

            SettingsMenuCanvasCollection.SetActive(true);
            BackgroundCanvas.SetActive(true);
            MainSettingsCanvas.SetActive(true);
            GraphicsCanvas.SetActive(false);
            AudioCanvas.SetActive(false);

            Level.SetActive(false);
            Dreamer.SetActive(false);
            Spawn.SetActive(false);
            Bounds.SetActive(false);
        }
    }
}
