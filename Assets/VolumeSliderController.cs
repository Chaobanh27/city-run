using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSliderController: MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string audioParam;
    [SerializeField] private float multiplier = 25;

    public void SetupSlider()
    {
        slider.onValueChanged.AddListener(SliderValue);
        slider.minValue = .001f;
        slider.value = PlayerPrefs.GetFloat(audioParam, slider.value);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(audioParam, slider.value);
    }

    private void SliderValue(float value)
    {
        audioMixer.SetFloat(audioParam, Mathf.Log10(value) * multiplier);
    }
}
