using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string MasterVol = "MasterVolume";
    private static readonly string MusicVol = "MusicVolume";
    private static readonly string SFXVol = "SFXVolume";

    private int firstPlayInt;
    private float musicVol, sfxVol, masterVol;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource[] master;
    [SerializeField] private AudioSource[] musicAudios;
    [SerializeField] private AudioSource[] sfxAudios;

    [Header("Audio Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;


    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if (firstPlayInt == 0)
        {
            masterVol = 0.3f;
            musicVol = 0.3f;
            sfxVol = 0.3f;
            masterSlider.value = masterVol;
            musicSlider.value = musicVol;
            sfxSlider.value = sfxVol;
            PlayerPrefs.SetFloat(MasterVol, masterVol);
            PlayerPrefs.SetFloat(MusicVol, musicVol);
            PlayerPrefs.SetFloat(SFXVol, sfxVol);
            PlayerPrefs.SetInt(FirstPlay, -1);
        }
        else
        {
            masterVol = PlayerPrefs.GetFloat(MasterVol);
            masterSlider.value = masterVol;
            musicVol = PlayerPrefs.GetFloat(MusicVol);
            musicSlider.value = musicVol;
            sfxVol = PlayerPrefs.GetFloat(SFXVol);
            sfxSlider.value = sfxVol;
        }
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(MasterVol, masterSlider.value);
        PlayerPrefs.SetFloat(MusicVol, musicSlider.value);
        PlayerPrefs.SetFloat(SFXVol, sfxSlider.value);
    }

    public void UpdateVolume()
    {
        for (int i = 0; i < master.Length; i++)
        {
            master[i].volume = masterSlider.value;
        }

        for (int i = 0; i < musicAudios.Length; i++)
        {
            musicAudios[i].volume = musicSlider.value;
        }

        for (int i = 0; i < sfxAudios.Length; i++)
        {
            sfxAudios[i].volume = sfxSlider.value;
        }
    }
}