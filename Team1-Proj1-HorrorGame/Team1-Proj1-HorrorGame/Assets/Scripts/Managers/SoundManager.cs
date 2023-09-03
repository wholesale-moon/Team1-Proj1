using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System;

public class SoundManager : MonoBehaviour
{
    //public const string prefAudioMute = "prefAudioMute";
    public AudioMixer audioMixer;
    public GameSaveData _GameSaveData;
    
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup soundEffectsMixerGroup;

    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider sfxVolume;
    
    [Space(10)]
    [SerializeField] private Sound[] sounds;

    private List<Sound> musicList= new List<Sound>();

    private void Awake()
    {
        //if(PlayerPrefs.HasKey(prefAudioMute))
        //    AudioListener.volume = PlayerPrefs.GetFloat(prefAudioMute);
        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.isLoop;
            s.source.playOnAwake = s.playOnAwake;
            s.source.volume = s.volume;

            switch (s.audioType)
            {
                case Sound.AudioTypes.soundEffect:
                    s.source.outputAudioMixerGroup = soundEffectsMixerGroup;
                    break;
                
                case Sound.AudioTypes.music:
                    s.source.outputAudioMixerGroup = musicMixerGroup;
                    musicList.Add(s);
                    break;
            }

            if (s.playOnAwake)
                s.source.Play();
        }
    }

    public void Start()
    {
        audioMixer.SetFloat("MasterVolume", _GameSaveData._masterVolume);
        audioMixer.SetFloat("MusicVolume", _GameSaveData._musicVolume);
        audioMixer.SetFloat("SFXVolume", _GameSaveData._sfxVolume);
        masterVolume.value = PlayerPrefs.GetFloat("MasterVolume");
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxVolume.value = PlayerPrefs.GetFloat("SFXVolume");
    }

    public void PlayClipByName(string _clipName)
    {
        // Finds sound clip in array with matching name
        Sound soundToPlay = Array.Find(sounds, dummySound => dummySound.clipName == _clipName);

        if (soundToPlay != null)
        {
            if (soundToPlay.audioType == Sound.AudioTypes.music)
            {
                StopAllMusic();
                soundToPlay.source.Play();
            }
            else
            {
                soundToPlay.source.Play();
            }
        }
    }

    public void StopClipByName(string _clipName)
    {
        // Finds sound clip in array with matching name
        Sound soundToStop = Array.Find(sounds, dummySound => dummySound.clipName == _clipName);
        
        if (soundToStop != null)
            soundToStop.source.Stop();
    }

    public void StopAllMusic()
    {
        foreach (Sound m in musicList)
        {
            if (m.source.isPlaying)
                m.source.Stop();
        }
    }
    
    public void ToggleMute()
    {
        if (AudioListener.volume == 1)
            AudioListener.volume = 0;
        else
            AudioListener.volume = 1;

        //PlayerPrefs.SetFloat(prefAudioMute, AudioListener.volume);
    }
}
