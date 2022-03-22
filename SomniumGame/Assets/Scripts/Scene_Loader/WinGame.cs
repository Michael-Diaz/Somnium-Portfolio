using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WinGame : MonoBehaviour
{
    public bool winCondition;

    public GameObject MainCamera;
    public GameObject PauseMenuCamera;

    public GameObject Level;
    public GameObject Dreamer;

    public GameObject EndGameCanvasCollection;
    public GameObject BackgroundCanvas;
    public GameObject WinCanvas;


    // Listener
    void Update()
    {
        if (winCondition)
        {
            // Switch cameras
            MainCamera.SetActive(false);
            PauseMenuCamera.SetActive(true);

            // Activate canvas objects
            EndGameCanvasCollection.SetActive(true);
            BackgroundCanvas.SetActive(true);
            WinCanvas.SetActive(true);

            // Deactivate level objects
            Level.SetActive(false);
            Dreamer.SetActive(false);
        }
    }
}
