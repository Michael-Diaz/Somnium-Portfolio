using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenCloseSettings : MonoBehaviour
{
    public bool additive=false;
    public bool setActive=false;

    public void OpenSettingsScene(string sceneToLoad)
    {
        // Prepare to load the currently active scene if no scene was specified
		if (sceneToLoad == null)
		{
			sceneToLoad = SceneManager.GetActiveScene().name;
		}

        // Load the scene
		SceneManager.LoadScene(sceneToLoad, additive ? LoadSceneMode.Additive : 0);

        // If this scene is also to be set as the active scene...
		if (setActive)
		{
            // Mark it active we have to wait a frame for it to load
			CallAfterDelay.Create(0, () => {
				SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));
			});
		}
    }

    public void CloseSettingsScene(string sceneToUnload)
    {
        SceneManager.UnloadSceneAsync(sceneToUnload);
    }
}
