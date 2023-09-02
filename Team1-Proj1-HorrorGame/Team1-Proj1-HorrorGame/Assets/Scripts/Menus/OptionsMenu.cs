using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public GameObject _SceneManager;
    
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
        _SceneManager.GetComponent<SoundManager>().PlayClipByName("Button Highlight");
    }

    // public void SetBrightness(float brightness)
    // {
    //      
    // }

    // public void SetClownMode(bool isClown)
    // {
    //     _GameSaveData._isClown = isClown;
    //     _SceneManager.GetComponent<SoundManager>().PlayClipByName("Button Highlight");
    // }

    // public void SkipPrologue(bool skipPrologue)
    // {
    //     _GameSaveData._skipPrologue = skipPrologue;
    //     _SceneManager.GetComponent<SoundManager>().PlayClipByName("Button Highlight");
    // }
}
