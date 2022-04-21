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
    public GameObject HUD;

    public GameObject EndGameCanvasCollection;
    public GameObject BackgroundCanvas;
    public GameObject WinCanvas;


    // Listener
    void Update()
    {
        if (winCondition)
        {
            StartCoroutine(WinGameRoutine());
        }
    }

    IEnumerator WinGameRoutine()
    {
        // Wait half a second so it isn't totally abrupt
        yield return new WaitForSeconds(0.25f);

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
        HUD.SetActive(false);
        Dreamer.transform.GetChild(4).gameObject.SetActive(false);
    }
}
