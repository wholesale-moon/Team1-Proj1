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
    
    [Header("Dialoge/Quest Triggers")]
    [SerializeField] private GameObject pickupTutorial;
    [SerializeField] private GameObject flashlightTutorial;
    [SerializeField] private GameObject blockedHouseDialogue;
    [SerializeField] private GameObject noFlashlighNoExit;

    [Header("Story Items")]
    [SerializeField] private GameObject generator;

    [Header("Doors")]
    [SerializeField] private GameObject houseDoor;
    
    [Header("HUD")]
    [SerializeField] GameObject itemHold;
    public GameObject flashlight;

    [Header("MiniCrows")]
    [SerializeField] private GameObject minicrowMorph;
    [SerializeField] private GameObject minicrow1;

    [HideInInspector] public bool hasMorphed = false;

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
            _GameSaveData.isPlayingCutscene = true;
            _GameSaveData._isHouseOpen = false;
        } else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            _GameSaveData._currentCutscene = 1;
            _GameSaveData.isPlayingCutscene = true;
        }

        minicrow1.GetComponent<Animator>().SetBool("Sludge3", true);
    }

    public void PickupTutorialActive()
    {
        pickupTutorial.SetActive(true);
    }

    // For prologue no house entry until first 2 quests are complete
    public void HouseBlockedDialogue()
    {
        blockedHouseDialogue.SetActive(true);
        Invoke("disableBlockedHouseDialogue", 0.1f);
    }

    private void disableBlockedHouseDialogue()
    {
        blockedHouseDialogue.SetActive(false);
    }
    
    public void GainFlashlight()
    {
        _GameSaveData._hasFlashlight = true;
        itemHold.SetActive(true);
        flashlight.SetActive(true);
        flashlightTutorial.SetActive(true);
        noFlashlighNoExit.SetActive(false);
    }

    public void Morph()
    {
        hasMorphed = true;
        minicrowMorph.GetComponent<Animator>().SetTrigger("Morph");
    }

    public void GeneratorOn()
    {
        generator.GetComponent<Animator>().SetTrigger("isTurnedOn");
    }
}
