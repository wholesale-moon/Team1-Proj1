using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;

public class SceneActivation : MonoBehaviour
{
    // This script is going to handle the activation and disabling of scene objects
    // based on the storyline of the game. Gonna take the load off player movement
    
    [Header("Game Save Data")]
    [SerializeField] private GameSaveData _GameSaveData;
    [SerializeField] GameObject _SceneManager;
    
    [Header("Prologue")]
    [SerializeField] private GameObject pickupTutorial;
    [SerializeField] private GameObject MedBottle;
    
    [Header("Level 1")]
    [SerializeField] private GameObject flashlightTutorial;
    [SerializeField] private GameObject blockedHouseDialogue;
    [SerializeField] private GameObject noFlashlighNoExit;
    [SerializeField] private GameObject LockedBarn;
    [SerializeField] private GameObject MorphCutscene;
    [SerializeField] private GameObject HeadHome;
    [SerializeField] private GameObject Lvl2Start;

    [Header("Lights")]
    [SerializeField] private GameObject houseLight;
    [SerializeField] private GameObject upstairsHouseLight;
    [SerializeField] private GameObject barnLight;
    [SerializeField] private GameObject shedLight;
    [SerializeField] private GameObject exteriorHouseLights;
    [SerializeField] private GameObject exteriorBarnLight;
    [SerializeField] private GameObject exteriorShedLight;
    [SerializeField] private GameObject baseStringLights;
    
    [Header("Level 2")]
    [SerializeField] private GameObject NeedStringLightsDialogue;
    [SerializeField] private GameObject PutUpStringLights;
    [SerializeField] private GameObject OutOfStringLightsDialogue;
    [SerializeField] private GameObject SLForShow;
    [SerializeField] private GameObject SLInteract;
    [SerializeField] private GameObject BurnCutsceneTrigger;
    [SerializeField] private GameObject StringLightSpecial;
    public GameObject StringLightBroken;

    [Header("Story Items")]
    [SerializeField] private GameObject generator;
    [SerializeField] private GameObject ScreenText;
    [SerializeField] private GameObject StartingQuest;

    [Header("Doors")]
    [SerializeField] private GameObject houseDoor;
    [SerializeField] private GameObject barnTransition;
    [SerializeField] private GameObject exteriorBarnDoors;
    
    [Header("HUD")]
    [SerializeField] GameObject itemHold;
    public GameObject flashlight;

    [Header("MiniCrows")]
    [SerializeField] private GameObject minicrowMorph;
    [SerializeField] private GameObject minicrow1;
    [SerializeField] private GameObject minicrow2;
    [SerializeField] private GameObject minicrow3;
    [SerializeField] private GameObject minicrow4;

    [HideInInspector] public bool hasMorphed = false;
    [HideInInspector] public int stringLightInventory = 0;

    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
        {
            _GameSaveData._hasFlashlight = false;
            _GameSaveData._hasBarnKey = false;
            _GameSaveData._hasStringLights = false;
            hasMorphed = false;
        }

        if (SceneManager.GetActiveScene().buildIndex == 1) //Prologue
        {
            _GameSaveData._currentCutscene = 0;
            _GameSaveData.isPlayingCutscene = true;
            _GameSaveData._isHouseOpen = false;
        } else if (SceneManager.GetActiveScene().buildIndex == 2) // Lvl 1
        {
            _GameSaveData._currentCutscene = 1;
            _GameSaveData.isPlayingCutscene = true;
            minicrow2.GetComponent<Animator>().SetBool("Sludge1", true);
        } else if (SceneManager.GetActiveScene().buildIndex == 3) // Lvl 2
        {
            _GameSaveData._hasFlashlight = true;
            _GameSaveData._hasBarnKey = true;
            _GameSaveData._isHouseOpen = true;
            _GameSaveData._hasStringLights = false;
            _GameSaveData.isPlayingCutscene = false;
            generator.GetComponent<Animator>().SetTrigger("isTurnedOn");
            minicrow2.GetComponent<Animator>().SetBool("Sludge2", true);
            minicrow3.GetComponent<Animator>().SetBool("Sludge1", true);
            StartCoroutine(TimeCheck());
        } else if (SceneManager.GetActiveScene().buildIndex == 4) // Lvl 3
        {
            // _GameSaveData._hasFlashlight = true;
            // _GameSaveData._hasBarnKey = true;
            // _GameSaveData._isHouseOpen = true;
            // generator.GetComponent<Animator>().SetTrigger("isTurnedOn");
            // minicrow2.GetComponent<Animator>().SetBool("Sludge3", true);
            // minicrow3.GetComponent<Animator>().SetBool("Sludge2", true);
            // StartCoroutine(TimeCheck());
        }

        minicrow1.GetComponent<Animator>().SetBool("Sludge3", true);
    }

    public void PickupTutorialActive()
    {
        pickupTutorial.SetActive(true);
        MedBottle.SetActive(true);
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
        Invoke("StartMorph", 1);
        _SceneManager.GetComponent<SoundManager>().PlayClipByName("EnemyGrowl");
    }

    private void StartMorph()
    {
        MorphCutscene.SetActive(true);
    }

    public void GeneratorOn()
    {
        houseLight.SetActive(true);
        upstairsHouseLight.SetActive(true);
        barnLight.SetActive(true);
        shedLight.SetActive(true);
        exteriorHouseLights.SetActive(true);
        exteriorBarnLight.SetActive(true);
        exteriorShedLight.SetActive(true);
        baseStringLights.SetActive(true);
        HeadHome.SetActive(true);
        barnTransition.SetActive(true);
        Lvl2Start.SetActive(true);
        
        generator.GetComponent<Animator>().SetTrigger("isTurnedOn");
    }

    public void GainKey()
    {
        Destroy(LockedBarn);
    }
    
    public void UnlockBarn()
    {
        exteriorBarnDoors.SetActive(true);
    }

    public IEnumerator TimeCheck()
    {
        yield return new WaitForSeconds(1.5f);

        ScreenText.SetActive(false);
        StartingQuest.SetActive(true);
        yield return null;
    }

    public void GainStringLights()
    {
        stringLightInventory = 12;
        NeedStringLightsDialogue.SetActive(false);
        PutUpStringLights.SetActive(true);
    }

    public void RanOutOfStringLights()
    {
        OutOfStringLightsDialogue.SetActive(true);
        SLForShow.SetActive(false);
        SLInteract.SetActive(true);
    }

    public void Burn()
    {
        BurnCutsceneTrigger.SetActive(true);
        StringLightSpecial.SetActive(true);
    }
}
