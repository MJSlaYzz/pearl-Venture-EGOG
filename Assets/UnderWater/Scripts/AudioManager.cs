using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioMixer gameMixer;
    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "sfxVolume";
    public const string Background_KEY = "backgroundVolume";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadVolume();
    }

    void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        float backgroundVolume = PlayerPrefs.GetFloat(Background_KEY, 1f);


        gameMixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        gameMixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
        gameMixer.SetFloat(VolumeSettings.MIXER_Background, Mathf.Log10(backgroundVolume) * 20);
    }

}
