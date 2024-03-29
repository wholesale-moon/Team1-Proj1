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
    [SerializeField] private GameObject blockedHouseDialogue;
    public GameObject houseTransition;
    
    [Header("Level 1")]
    [SerializeField] private GameObject flashlightTutorial;
    [SerializeField] private GameObject noFlashlighNoExit;
    [SerializeField] private GameObject LockedBarn;
    [SerializeField] private GameObject GeneratorBroken;
    [SerializeField] private GameObject ToolInteraction;
    [SerializeField] private GameObject ToolsSprite;
    [SerializeField] private GameObject MorphCutscene;
    [SerializeField] private GameObject MorphCutscene2;
    [SerializeField] private GameObject HeadHome;
    [SerializeField] private GameObject Lvl2Start;

    [Space(10)]
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
    [SerializeField] private GameObject FixStringLightsDialogue;
    [SerializeField] private GameObject ExitQuest;
    [SerializeField] private GameObject FlameToolUpstairs;
    [SerializeField] private GameObject Lvl3Start;

    [Header("Level 3")]
    [SerializeField] private GameObject NeedFlameTool;
    [SerializeField] private GameObject protectTheFarm;
    [SerializeField] private GameObject SafeToHeadHome;
    [SerializeField] private GameObject creditsStart;

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
    public GameObject Inventory;

    [Header("MiniCrows")]
    [SerializeField] private GameObject minicrowMorph;
    [SerializeField] private GameObject minicrow1;
    [SerializeField] private GameObject minicrow2;
    [SerializeField] private GameObject minicrow3;
    [SerializeField] private GameObject minicrow4;

    [HideInInspector] public bool hasMorphed = false;
    public int stringLightInventory = 13;
    [HideInInspector] public bool clearLvl3 = false;

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
            houseTransition.SetActive(false);
        } 
        else if (SceneManager.GetActiveScene().buildIndex == 2) // Lvl 1
        {
            _GameSaveData._currentCutscene = 1;
            _GameSaveData.isPlayingCutscene = true;
            _GameSaveData._hasTools = false;
            _GameSaveData._isHouseOpen = true;
            minicrow2.GetComponent<Animator>().SetBool("Sludge1", true);
        } 
        else if (SceneManager.GetActiveScene().buildIndex == 3) // Lvl 2
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
        } 
        else if (SceneManager.GetActiveScene().buildIndex == 4) // Lvl 3
        {
            _GameSaveData._hasFlashlight = true;
            _GameSaveData._hasBarnKey = true;
            _GameSaveData._isHouseOpen = true;
            _GameSaveData._hasStringLights = false;
            _GameSaveData._hasFlameTool = false;
            _GameSaveData._numOfScarecrows = 7;
            generator.GetComponent<Animator>().SetTrigger("isTurnedOn");
            minicrow2.GetComponent<Animator>().SetBool("Sludge3", true);
            minicrow3.GetComponent<Animator>().SetBool("Sludge2", true);
            StartCoroutine(TimeCheck());
        }

        minicrow1.GetComponent<Animator>().SetBool("Sludge3", true);
    }

    void Update()
    {
        if (_GameSaveData._isHouseOpen == true)
        {
            houseTransition.SetActive(true);
        }

        if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            if (stringLightInventory == 0)
            {
                RanOutOfStringLights();
            }
        }
        
        if(SceneManager.GetActiveScene().buildIndex == 4)
        {
            if(_GameSaveData._numOfScarecrows == 0 || _GameSaveData._numOfScarecrows <= 0)
            {
                clearLvl3 = true;
            }
        }

        if(clearLvl3)
        {
            SafeToHeadHome.SetActive(true);
            creditsStart.SetActive(true);
        }
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

    public void GeneratorBrokenDialogue()
    {
        GeneratorBroken.SetActive(true);
        ToolInteraction.SetActive(true);
        ToolsSprite.SetActive(false);
        MorphCutscene2.SetActive(true);
        Invoke("disableBlockedHouseDialogue", 0.1f);
    }

    private void disableGeneratorBroken()
    {
        GeneratorBroken.SetActive(false);
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
        Lvl2Start.SetActive(true);
        
        generator.GetComponent<Animator>().SetTrigger("isTurnedOn");
        generator.GetComponent<AudioSource>().enabled = true;
    }

    public void GainKey()
    {
        _SceneManager.GetComponent<DialogueManager>().DialogueBox.SetActive(false);
        barnTransition.SetActive(true);
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
        Inventory.SetActive(true);
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
        FixStringLightsDialogue.SetActive(true);
        FlameToolUpstairs.SetActive(true);
        Lvl3Start.SetActive(true);
        _SceneManager.GetComponent<DialogueManager>().DialogueBox.SetActive(false);
    }

    public void ExitQuestActive()
    {
        ExitQuest.SetActive(true);
    }

    public void ProtectTheFarm()
    {
        NeedFlameTool.SetActive(false);
        protectTheFarm.SetActive(true);
    }
}
