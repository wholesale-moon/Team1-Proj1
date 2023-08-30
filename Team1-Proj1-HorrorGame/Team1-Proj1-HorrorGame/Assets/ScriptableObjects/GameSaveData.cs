using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(fileName = "GameSaveData", menuName = "ScriptableObjects/GameSaveData")]
public class GameSaveData : ScriptableObject
{
    [Header("Player Data")]
    public bool hasFlashlight;
    public bool hasBarnKey;
    
    [Header("Audio Data")]
    public float _masterVolume;
    public float _musicVolume;
    public float _sfxVolume;
    
    [Header("Cutscene Data")]
    public int _currentCutscene;
    public float[] _cutsceneEndTimes;
}
