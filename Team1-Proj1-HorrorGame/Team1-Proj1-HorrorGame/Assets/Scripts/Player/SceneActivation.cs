using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneActivation : MonoBehaviour
{
    // This script is going to handle the activation and disabling of scene objects
    // based on the storyline of the game. Gonna take the load off player movement
    
    [Header("Game Save Data")]
    [SerializeField] private GameSaveData _GameSaveData;
    
    [Header("Scene Objects")]
    // Dialogue/Quest Triggers
    [SerializeField] private GameObject flashlighTutTrigger;

    //Doors
    [SerializeField] private GameObject houseDoor;
    
    //HUD
    [SerializeField] GameObject itemHold;
    public GameObject flashlight;

    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
        {
            _GameSaveData._hasFlashlight = false;
            _GameSaveData._hasBarnKey = false;
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            _GameSaveData._currentCutscene = 0;
        } else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            _GameSaveData._currentCutscene = 2;
        }
    }

    public void GainFlashlight()
    {
        _GameSaveData._hasFlashlight = true;
        itemHold.SetActive(true);
        flashlight.SetActive(true);
        flashlighTutTrigger.SetActive(true);
    }
}
