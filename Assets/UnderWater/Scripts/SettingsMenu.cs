using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Dropdown resoluitionDropdown;
    //public AudioMixer audioMixer;
    Resolution[] resolutions;
    private void Start()
    {
        GetResoluitionValues();
    }
    
    /*public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Waves",volume);
        
    }*/
    public void SetQuality(int qualityIndex)
    {
        Debug.Log("QUALITY CHANGE!");
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Debug.Log("FullScreen is on!");
        Screen.fullScreen = isFullscreen;
    }
    public void SetResoluition(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,resolution.height,Screen.fullScreen);
    }
    public void GetResoluitionValues()
    {
        resolutions = Screen.resolutions;
        resoluitionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resoluitionDropdown.AddOptions(options);
        resoluitionDropdown.value = currentResolutionIndex;
        resoluitionDropdown.RefreshShownValue();
    }
}
