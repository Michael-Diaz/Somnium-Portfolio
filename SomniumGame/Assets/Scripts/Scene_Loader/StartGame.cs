using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject loadingScreenCanvas;
    public Slider slider;


    public void NewGame(string levelToLoad) 
    {
        // Get the old (current) scene
        Scene old_scene = SceneManager.GetActiveScene();
        Debug.Log($"Current Active Scene: {old_scene.name}");
        
        // Load new scene
        StartCoroutine(LoadAsynchronously(levelToLoad, old_scene));
    }


    IEnumerator LoadAsynchronously(string levelToLoad, Scene old_scene)
    {
        // Set the loading of the level as an async operation
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Single);

        while(!operation.isDone)
        {
            // Get the 0-1 progress value
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            // Apply the progress value to the slider, which also goes from 0-1
            slider.value = progress;

            // Going to have a thing here to show the percentage completed as text

            // Unload the main menu
            yield return UnloadAsynchronously(levelToLoad, old_scene);
        }
    }


    IEnumerator UnloadAsynchronously(string levelToLoad, Scene old_scene)
    {
        // See what scenes are loaded
        int countLoaded = SceneManager.sceneCount;
        for (int i = 0; i < countLoaded; i++)
        {
            Scene loadedScene = SceneManager.GetSceneAt(i);
            Debug.Log($"Currently Loaded Scenes: {loadedScene.name}");
        }

        // Set the newly loaded scene as active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelToLoad));
        Scene cur_scene = SceneManager.GetActiveScene();
        Debug.Log($"Current Active Scene: {cur_scene.name}");

        // Set the unloading of the level as an async operation
        AsyncOperation operation = SceneManager.UnloadSceneAsync(old_scene);

        while(!operation.isDone)
        {
            // End the Coroutine
            yield return null;
        }
    }
}
