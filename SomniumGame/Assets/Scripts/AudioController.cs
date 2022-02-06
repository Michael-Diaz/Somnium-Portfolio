using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] Slider _slider;
    [SerializeField] AudioMixer _mixer;
    [SerializeField] string _volumeParameter = "MasterVolume";

    private void Awake()
    {
        _slider.onValueChanged.AddListener(HandleSliderValueChanged);
    }

    private void HandleSliderValueChanged(float value)
    {
        _mixer.SetFloat(_volumeParameter, Mathf.Log10(value) * 30f);
    }

    private void OnBack()
    {
        PlayerPrefs.SetFloat(_volumeParameter, _slider.value);
    }

    void Start()
    {
        _slider.value = PlayerPrefs.GetFloat(_volumeParameter, _slider.value);
    }
}
