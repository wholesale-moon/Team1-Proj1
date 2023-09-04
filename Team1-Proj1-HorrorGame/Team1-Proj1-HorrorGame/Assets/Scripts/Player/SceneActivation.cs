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
    
    [Header("Dialogue/Quest Triggers")]
    [SerializeField] private GameObject pickupTutorial;
    [SerializeField] private GameObject flashlightTutorial;
    [SerializeField] private GameObject blockedHouseDialogue;
    [SerializeField] private GameObject noFlashlighNoExit;
    [SerializeField] private GameObject LockedBarn;
    [SerializeField] private GameObject HeadHome;
    [SerializeField] private GameObject NeedStringLightsDialogue;
    [SerializeField] private GameObject PutUpStringLights;

    [Header("Story Items")]
    [SerializeField] private GameObject generator;
    [SerializeField] private GameObject MorphCutscene;
    [SerializeField] private GameObject Lvl2Start;
    [SerializeField] private GameObject ScreenText;
    [SerializeField] private GameObject StartingQuest;

    [Header("Lights")]
    [SerializeField] private GameObject houseLight;
    [SerializeField] private GameObject upstairsHouseLight;
    [SerializeField] private GameObject barnLight;
    [SerializeField] private GameObject shedLight;
    [SerializeField] private GameObject exteriorHouseLights;
    [SerializeField] private GameObject exteriorBarnLight;
    [SerializeField] private GameObject exteriorShedLight;
    [SerializeField] private GameObject baseStringLights;


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

    [HideInInspector] public bool hasMorphed = false;

    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2)
        {
            _GameSaveData._hasFlashlight = false;
            _GameSaveData._hasBarnKey = false;
            _GameSaveData._hasStringLights = false;
            hasMorphed = false;
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
        } else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            _GameSaveData._hasFlashlight = true;
            _GameSaveData._hasBarnKey = true;
            _GameSaveData._isHouseOpen = true;
            generator.GetComponent<Animator>().SetTrigger("isTurnedOn");
            StartCoroutine(TimeCheck());
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
        NeedStringLightsDialogue.SetActive(false);
        PutUpStringLights.SetActive(true);
    }
}
