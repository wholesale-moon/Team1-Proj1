using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public GameObject _SceneManager;
    
    public AudioMixer audioMixer;
    public GameSaveData _GameSaveData;
    
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider sfxVolume;
    
    public void SetMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume.value);
        audioMixer.SetFloat("MasterVolume", volume);
        _GameSaveData._masterVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume.value);
        audioMixer.SetFloat("MusicVolume", volume);
        _GameSaveData._musicVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume.value);
        audioMixer.SetFloat("SFXVolume", volume);
        _GameSaveData._sfxVolume = volume;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        _SceneManager.GetComponent<SoundManager>().PlayClipByName("Button Highlight");
    }

    public void SkipPrologue(bool skipPrologue)
    {
        _GameSaveData._skipPrologue = skipPrologue;
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
}
