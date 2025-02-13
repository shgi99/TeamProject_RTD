using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UISoundOption : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicMasterSlider;
    [SerializeField] private Slider musicBGMSlider;
    [SerializeField] private Slider musicSFXSlider;
    [SerializeField] private Button quitButton;
    // Start is called before the first frame update
    void Start()
    {
        musicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicBGMSlider.onValueChanged.AddListener(SetMusicVolume);
        musicSFXSlider.onValueChanged.AddListener(SetSFXVolume);
        quitButton.onClick.AddListener(() => optionPanel.SetActive(false));
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
