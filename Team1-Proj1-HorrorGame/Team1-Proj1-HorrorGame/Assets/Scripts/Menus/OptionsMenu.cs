using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public GameSaveData _GameSaveData;
    
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        _GameSaveData._masterVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
        _GameSaveData._musicVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
        _GameSaveData._sfxVolume = volume;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
