using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoseGame : MonoBehaviour
{
    public bool loseCondition;

    public GameObject MainCamera;
    public GameObject PauseMenuCamera;

    public GameObject Level;
    public GameObject Dreamer;

    public GameObject EndGameCanvasCollection;
    public GameObject BackgroundCanvas;
    public GameObject LoseCanvas;


    // Listener
    void Update()
    {
        if (loseCondition)
        {
            // Switch cameras
            MainCamera.SetActive(false);
            PauseMenuCamera.SetActive(true);

            // Activate canvas objects
            EndGameCanvasCollection.SetActive(true);
            BackgroundCanvas.SetActive(true);
            LoseCanvas.SetActive(false);

            // Deactivate level objects
            Level.SetActive(false);
            Dreamer.SetActive(false);
        }
    }
}
