using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [Header("Graphics Settings")]
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Resolution[] resolutions;
    //[SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Slider brightnessSlider;

    // Start is called before the first frame update
    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> choices = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string choice = resolutions[i].width + "x" + resolutions[i].height;
            choices.Add(choice);

            if ((resolutions[i].width == Screen.currentResolution.width) && (resolutions[i].height == Screen.currentResolution.height))
            {
                currentResolutionIndex = i;
            }
        }


        resolutionDropdown.AddOptions(choices);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }


    public void setFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void setResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetBrightness(int brightness)
    {
        PlayerPrefs.SetInt("brightnessLevel", brightness);
    }
}
