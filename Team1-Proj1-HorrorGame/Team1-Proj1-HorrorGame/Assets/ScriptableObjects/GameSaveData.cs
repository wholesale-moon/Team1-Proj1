using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(fileName = "GameSaveData", menuName = "ScriptableObjects/GameSaveData")]
public class GameSaveData : ScriptableObject
{
    [Header("Player Data")]
    public bool _hasFlashlight;
    public bool _hasBarnKey;
    public bool _hasTools;
    public bool _hasStringLights;
    public bool _hasFlameTool;

    [Space(10)]
    public bool _hasLantern;
    public bool _hasGasCan;

    [Space(10)]
    public bool _isHouseOpen;

    [Header("Pause")]
    public bool _gamePause;
    
    [Header("Settings Data")]
    public float _masterVolume;
    public float _musicVolume;
    public float _sfxVolume;

    [Space(10)]
    public bool _skipPrologue;
    public bool _hasCompletedPrologue;
    
    [Header("Cutscene Data")]
    public bool isPlayingCutscene;
    
    public int _currentCutscene;
    public float[] _cutsceneEndTimes;


    public int _numOfScarecrows;
}
