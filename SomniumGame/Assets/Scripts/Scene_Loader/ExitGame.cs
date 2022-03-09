using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting game.");
    }
}
