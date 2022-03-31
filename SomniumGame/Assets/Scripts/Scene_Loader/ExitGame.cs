using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public GameObject UniversalWebcam;

    // Quit the game
    public void QuitGame()
    {
        // Stop the webcam
        if (UniversalWebcam.GetComponent<UniversalWebcam>().webcamTexture.isPlaying)
            UniversalWebcam.GetComponent<UniversalWebcam>().webcamTexture.Stop();

        // Quit out of the game
        Application.Quit();
        Debug.Log("Quitting game.");
    }
}
