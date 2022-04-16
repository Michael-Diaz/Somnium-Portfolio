using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
            StartCoroutine(LoseGameRoutine());
        }
    }

    IEnumerator LoseGameRoutine()
    {
        // Wait a bit so you can see what kills you
        yield return new WaitForSeconds(1.0f);

        // Switch cameras
        MainCamera.SetActive(false);
        PauseMenuCamera.SetActive(true);

        // Activate canvas objects
        EndGameCanvasCollection.SetActive(true);
        BackgroundCanvas.SetActive(true);
        LoseCanvas.SetActive(true);

        // Deactivate level objects
        Level.SetActive(false);
        Dreamer.SetActive(false);
    }
}
