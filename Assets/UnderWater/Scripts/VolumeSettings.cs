using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    #region Variables
    [SerializeField] AudioMixer gameMixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Slider backgroundVolumeSlider;


    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";
    public const string MIXER_Background = "BackgroundVolume";
    #endregion

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        backgroundVolumeSlider.onValueChanged.AddListener(SetBackgroundVolume);
    }

     void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY,1f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);
        backgroundVolumeSlider.value = PlayerPrefs.GetFloat(AudioManager.Background_KEY, 1f);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, sfxVolumeSlider.value);
        PlayerPrefs.SetFloat(AudioManager.Background_KEY, backgroundVolumeSlider.value);
    }
    void SetMusicVolume(float value)
    {
        //Slider And Audio Mixer Have Different Range of numbers (Liner & Log) 
        gameMixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }
    void SetSFXVolume( float value)
    {
        gameMixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
    }
    void SetBackgroundVolume(float value)
    {
        gameMixer.SetFloat(MIXER_Background, Mathf.Log10(value) * 20);
    }

}
