using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;


    Resolution[] resoultions;
    
    public TMP_Dropdown resDropDown;
    public Slider masterVolumeSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle fullScreenToggle;

    private void Start()
    {
        resoultions = Screen.resolutions;
        resDropDown.ClearOptions();

        List<string> options = new List<string>();

        int currentRes = 0;
        for(int i =0; i < resoultions.Length; i++)
        {
            string option = resoultions[i].width + "x" + resoultions[i].height;
            options.Add(option);

            if (resoultions[i].width == Screen.currentResolution.width && resoultions[i].height == Screen.currentResolution.height)
            {
                currentRes = i;
            }

           
        }

        if(Screen.fullScreen)
        {
            fullScreenToggle.isOn = true;
        }

        else
        {
            fullScreenToggle.isOn=false;
        }
        resDropDown.AddOptions(options);
        resDropDown.value = currentRes;
        resDropDown.RefreshShownValue();

        Screen.SetResolution(resoultions[currentRes].width, resoultions[currentRes].height, fullScreenToggle.isOn);

        float currentMasterVolume;
        audioMixer.GetFloat("masterVolume", out currentMasterVolume);
        masterVolumeSlider.value = Mathf.Pow(10, currentMasterVolume / 20);

        float currentMusicVolume;
        audioMixer.GetFloat("musicVolume", out currentMusicVolume);
       musicSlider.value = Mathf.Pow(10, currentMusicVolume / 20);

        float currentSFXVolume;
        audioMixer.GetFloat("sfxVolume", out currentSFXVolume);
        sfxSlider.value = Mathf.Pow(10, currentSFXVolume / 20);;
    }
    public void SetVolume(float vol)
    {
        audioMixer.SetFloat("volume", vol);
    }

    public void setMasterVolume(float vol)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(masterVolumeSlider.value) * 20);
    }

    public void setMusicVolume(float vol)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(musicSlider.value) * 20);
    }

    public void setSFXVolume(float vol)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log10(sfxSlider.value) * 20);
    }
    public void setQuality(int qualIndex)
    {
        QualitySettings.SetQualityLevel(qualIndex);
    }

    public void  toggleFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

 
    }

    public void setResolution(int currentRes)
    {
        Resolution res = resoultions[currentRes];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

}
