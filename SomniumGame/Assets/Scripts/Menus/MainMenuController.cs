using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    //public Slider musicSlider, sfxSlider;

    [Header("Graphics Settings")]
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Resolution[] resolutions;
    [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Slider brightnessSlider;

    //private int _brightnessLevel;


    void Start()
    {
        //musicSlider.value = PlayerPrefs.GetFloat("music", 0.75f);
        //sfxSlider.value = PlayerPrefs.GetFloat("sfx", 0.75f);

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> choices = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string choice = resolutions[i].width + "x" + resolutions[i].height;
            choices.Add(choice);

            if ((resolutions[i].width == Screen.width) && (resolutions[i].height == Screen.height))
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(choices);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    
    public void SetBrightness(int brightness)
    {
        PlayerPrefs.SetInt("brightnessLevel", brightness);
    }

    public void SetQuality(int quality)
    {
        PlayerPrefs.SetInt("qualityLevel", quality);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
