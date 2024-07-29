using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_Setting : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [Header("SFX Setting")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxSliderText;
    [SerializeField] private string sfxParameter;

    [Header("BGM Setting")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private TextMeshProUGUI bgmSliderText;
    [SerializeField] private string bgmParameter;

    public void SFXSliderValue(float value) {
        sfxSliderText.text = Mathf.RoundToInt(value * 100) + "%";
        float newValue = value * 20;
        audioMixer.SetFloat(sfxParameter, newValue);
    }

    public void BGMSliderValue(float value) {
        bgmSliderText.text = Mathf.RoundToInt(value * 100) + "%";
        float newValue = value * 20;
        audioMixer.SetFloat(bgmParameter, newValue);
    }
    private void OnDisable() {
        PlayerPrefs.SetFloat(sfxParameter, sfxSlider.value);
        PlayerPrefs.SetFloat(bgmParameter, bgmSlider.value);
    }
    private void OnEnable() {
        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter, 0.7f);
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, 0.7f);
    }
}
