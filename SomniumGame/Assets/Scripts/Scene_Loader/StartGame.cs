using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Start a new game
    public void StartNewGame()
    {
        // Since this is a new game, this is all that's needed
        SceneManager.LoadSceneAsync("LevelGen"); // load the Scene in the background
    }
}
