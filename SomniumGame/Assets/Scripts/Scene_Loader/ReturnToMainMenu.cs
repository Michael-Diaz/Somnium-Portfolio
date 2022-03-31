using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnToMainMenu : MonoBehaviour
{
	public GameObject UniversalWebcam;
    public string sceneToLoad;
    public string sceneToUnload;
    public bool loadAsync=false;
    public bool additive=false;
    public bool setActive=false;


    public void LoadScene()
	{
		// Stop the webcam
        if (UniversalWebcam.GetComponent<UniversalWebcam>().webcamTexture.isPlaying)
            UniversalWebcam.GetComponent<UniversalWebcam>().webcamTexture.Stop();
			
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

            // Unload old scene after we wait a frame
			CallAfterDelay.Create(0, () => {
				UnloadScene();
			});
		}
	}


	public void UnloadScene()
	{
		SceneManager.UnloadSceneAsync(sceneToUnload);
    }
}
