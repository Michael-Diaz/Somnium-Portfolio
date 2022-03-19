using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public GameObject originalCanvas;
    public GameObject loadingScreenCanvas;
    public Slider slider;
    public string levelToLoad;
    public string levelToUnload;
    public bool loadAsync=false;
    public bool additive=false;
    public bool setActive=false;


    public void LoadScene()
	{
        // Prepare to load the currently active scene if no scene was specified
		if (levelToLoad == null)
		{
			levelToLoad = SceneManager.GetActiveScene().name;
		}

        // Load the scene
        // If it was specified to load asyncronously, it expects a loading scene to have been set up
        // Otherwise, you only need to care about the additive variable
        if (loadAsync)
        {
            StartCoroutine(LoadAsynchronously());
        }
        else
		{
		    SceneManager.LoadScene(levelToLoad, additive ? LoadSceneMode.Additive : 0);
		}


        // If this scene is also to be set as the active scene...
		if (setActive)
		{
            // Mark it active we have to wait a frame for it to load
			CallAfterDelay.Create(0, () => {
				SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelToLoad));
			});

            // Unload old scene after we wait a frame
			CallAfterDelay.Create(0, () => {
				UnloadScene();
			});
		}
	}


    IEnumerator LoadAsynchronously()
    {
        // Set the loading of the level as an async operation
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelToLoad, additive ? LoadSceneMode.Additive : 0);

        // Deactivate the original canvas and activate the loading screen canvas
        originalCanvas.SetActive(false);
        loadingScreenCanvas.SetActive(true);

        while(!operation.isDone)
        {
            // Get the 0-1 progress value
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            // Apply the progress value to the slider, which also goes from 0-1
            slider.value = progress;

            // Going to have a thing here to show the percentage completed as text

            // End coroutine
            yield return null;
        }
    }


	public void UnloadScene()
	{
		SceneManager.UnloadSceneAsync(levelToUnload);
    }
}