using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using Cinemachine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Player")]
    public Rigidbody2D rb2d;
    private Vector2 moveInput;
    float moveSpeed = 5;
    public bool canMove = true;
    [SerializeField] private Animator topAnimator;
    [SerializeField] private Animator bottomAnimator;

    private bool canInteract;
    private GameObject interactable;

    [Header("HUD")]
    public TMP_Text questLog;
    [SerializeField] private GameObject itemHold;
    [SerializeField] private GameObject flashlightHold;
    [SerializeField] private GameObject lanternHold;
    [SerializeField] private GameObject flameToolHold;

    [Space(10)]
    [SerializeField] private TMP_Text actionText;

    [Header("Cameras")]
    public GameObject theCamera;
    public GameObject houseCamera;
    public GameObject barnCamera;
    public GameObject shedCamera;

    [Header("Audio")]
    [SerializeField] private AudioMixerGroup soundEffectsMixerGroup;
    public AudioSource walkSound;
    public AudioClip[] sounds;
    bool isMoving = false;

    [Header("Objects")]
    [SerializeField] private GameSaveData _GameSaveData;
    [SerializeField] private GameObject _SceneManager;
    [SerializeField] private GameObject sceneTransitioner;

    [Space(10)]
    public GameObject flashlight;
    [SerializeField] private GameObject FlameTool;
    [SerializeField] private GameObject flame;
    [SerializeField] private GameObject flameDamage;
    private bool canDamage = true;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        walkSound = gameObject.GetComponent<AudioSource>();
        walkSound.outputAudioMixerGroup = soundEffectsMixerGroup;
        // Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        OnMove();
        CheckAttack();
        CheckInteractables();
    }

    #region Player Movement
    void OnMove()
    {
        if (canMove == true)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            // Setting animation bools to determine which animation should play
            topAnimator.SetFloat("Horizontal", moveInput.x);
            topAnimator.SetFloat("Vertical", moveInput.y);
            topAnimator.SetFloat("Speed", moveInput.sqrMagnitude);

            bottomAnimator.SetFloat("Horizontal", moveInput.x);
            bottomAnimator.SetFloat("Vertical", moveInput.y);
            bottomAnimator.SetFloat("Speed", moveInput.sqrMagnitude);

            moveInput.Normalize();

            rb2d.velocity = moveInput * moveSpeed;

            if (rb2d.velocity.x != 0 || rb2d.velocity.y != 0)
            {

                isMoving = true;

                if (isMoving)
                {
                    if (!walkSound.isPlaying && _GameSaveData.isPlayingCutscene == false)
                    {
                        walkSound.Play();
                    }
                }
                else
                {
                    walkSound.Stop();
                }
            }
        }
        else if (canMove == false)
        {
            rb2d.velocity = Vector3.zero;
            // topAnimator.gameObject.GetComponent<Animator>().enabled = false;
            // bottomAnimator.gameObject.GetComponent<Animator>().enabled = false;
        }
       
    }
    #endregion

    #region Attack
    private void CheckAttack()
    {
        if (Input.GetMouseButtonDown(0) && _GameSaveData._hasFlameTool == true)
        {
            flame.GetComponent<Animator>().SetTrigger("Blast");
            StartCoroutine(DealDamage());
        }
    }

    private IEnumerator DealDamage()
    {
        yield return new WaitForSeconds(0.09f);

        flameDamage.SetActive(true);
        yield return new WaitForSeconds(1.11f);

        flameDamage.SetActive(false);
        StartCoroutine(DamageCooldown());
        yield return null;
    }

    private IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(5);

        canDamage = true;
        yield return null;
    }
    #endregion


    private void CheckInteractables()
    {
        if (Input.GetKeyDown(KeyCode.E) & canInteract)
        {
            if (_GameSaveData._hasFlashlight == false)
            {
                transform.GetComponent<SceneActivation>().GainFlashlight();
                interactable.SetActive(false);
                flashlightHold.SetActive(true);
                UpdateActionTextp("Flashlight");
                canInteract = false;
            } 
            else if (interactable.tag == "LanternStash") 
            {
                _GameSaveData._hasLantern = true;
                flashlightHold.SetActive(false);
                lanternHold.SetActive(true);
                UpdateActionTextp("Lantern");
            }
            else if (interactable.tag == "StringLightsPickup")
            {
                Destroy(interactable);
                if (_GameSaveData._hasStringLights == false)
                {
                    _GameSaveData._hasStringLights = true;
                    transform.GetComponent<SceneActivation>().GainStringLights();
                    UpdateActionTextp("x12 String Lights");
                } else {
                    transform.GetComponent<SceneActivation>().stringLightInventory = 2;
                    transform.GetComponent<SceneActivation>().Burn();
                    UpdateActionTextp("x2 String Lights");
                }
            }
            else if (interactable.tag == "PlaceStringLight")
            {
                if(transform.GetComponent<SceneActivation>().stringLightInventory > 0)
                {
                    transform.GetComponent<SceneActivation>().stringLightInventory -= 1;
                    interactable.GetComponent<StringLights>().stringLight.SetActive(true);
                    Destroy(interactable);
                } else if (transform.GetComponent<SceneActivation>().stringLightInventory == 0)
                {
                    transform.GetComponent<SceneActivation>().RanOutOfStringLights();
                }
            }
            else if (interactable.tag == "PlaceSpecialSL")
            {
                transform.GetComponent<SceneActivation>().stringLightInventory -= 1;
                interactable.GetComponent<StringLights>().stringLight.SetActive(true);
                transform.GetComponent<SceneActivation>().StringLightBroken.GetComponent<BreakLights>().FixStringLights();
                Destroy(interactable);
                Destroy(transform.GetComponent<SceneActivation>().StringLightBroken);
            }
            else if (interactable.tag == "Generator")
            {
                if(_GameSaveData._hasTools)
                {
                    Destroy(interactable);
                    transform.GetComponent<SceneActivation>().GeneratorOn();
                } else {
                    transform.GetComponent<SceneActivation>().GeneratorBrokenDialogue();
                }
            }
            else if (interactable.tag == "FlameTool")
            {
                if (canDamage)
                {
                    _GameSaveData._hasFlameTool = true;
                    Destroy(interactable);
                    FlameTool.SetActive(true);
                    flameToolHold.SetActive(true);
                    flashlightHold.SetActive(false);
                    flashlight.SetActive(false);
                    transform.GetComponent<SceneActivation>().ProtectTheFarm();
                    UpdateActionTextp("FlameTool");
                }
            }
            else if (interactable.tag == "Tools")
            {
                Destroy(interactable);
                _GameSaveData._hasTools = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {   
        #region Travel
        if (collision.gameObject.tag == "House")
        {
            if (_GameSaveData._isHouseOpen == true)
            {
                StartCoroutine(ToHouse());
            } else {
                transform.GetComponent<SceneActivation>().HouseBlockedDialogue();
            }
        }
        
        if (collision.gameObject.tag == "Barn")
        {
            if(_GameSaveData._hasBarnKey == true)
            {
                transform.GetComponent<SceneActivation>().UnlockBarn();
                StartCoroutine(ToBarn());
            } else {
                UpdateActionTextn("Barn Key");
            }
        }


        if (collision.gameObject.tag == "Shed")
        {
            StartCoroutine(ToShed());
        }
        
        if (collision.gameObject.tag == "Field")
        {
            StartCoroutine(ExitHouse());
        }
        
        if (collision.gameObject.tag == "Field2")
        {
            if (transform.GetComponent<SceneActivation>().hasMorphed == false)
            {
                transform.GetComponent<SceneActivation>().UnlockBarn();
                transform.GetComponent<SceneActivation>().Morph();
            }

            StartCoroutine(ExitBarn());
        }

        if (collision.gameObject.tag == "Field3")
        {
            StartCoroutine(ExitShed());
        }

        if (collision.gameObject.tag == "Downstairs")
        {
            StartCoroutine(ToUpstairs());
        }

        if (collision.gameObject.tag == "Upstairs")
        {
            StartCoroutine(ToDownstairs());
        }
        #endregion
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        #region Sound Triggers
        if (obj.gameObject.tag == "Corn")
        {
            walkSound.clip = sounds[1];
        }

        if (obj.gameObject.tag == "Grass")
        {
            walkSound.clip = sounds[2];
        }

        if (obj.gameObject.tag == "Dirt")
        {
            walkSound.clip = sounds[0];
        }
        #endregion

        #region Pickups
        if (obj.gameObject.tag == "BarnKey")
        {
            _GameSaveData._hasBarnKey = true;
            Destroy(obj.gameObject);
            transform.GetComponent<SceneActivation>().GainKey();
            UpdateActionTextp("Barn Key");
        }

        if (obj.gameObject.tag == "Medicine")
        {
            if (transform.GetComponent<PlayerHealth>().currentHealth != transform.GetComponent<PlayerHealth>().maxHealth)
            {
                transform.GetComponent<PlayerHealth>().Heal();
                StartCoroutine(transform.GetComponent<PlayerHealth>().MedicineSpawn());
                UpdateActionTextp("Medicine");
            }
        }
        
        if (obj.gameObject.tag == "GasCan")
        {
            UpdateActionTextp("Fuel");
            Destroy(obj.gameObject);
        }
        #endregion

        #region Interactables
        if (obj.gameObject.tag == "FlashlightPickup")
        {
            interactable = obj.gameObject;
            canInteract = true;
        }

        if (obj.gameObject.tag == "StringLightsPickup")
        {
            interactable = obj.gameObject;
            canInteract = true;
        }

        if (obj.gameObject.tag == "PlaceStringLight")
        {
            interactable = obj.gameObject;
            canInteract = true;
        }

        if (obj.gameObject.tag == "PlaceSpecialSL")
        {
            interactable = obj.gameObject;
            canInteract = true;
        }

        if (obj.gameObject.tag == "Lantern")
        {
            interactable = obj.gameObject;
            canInteract = true;
        }
        
        if (obj.gameObject.tag == "FlameTool")
        {
            interactable = obj.gameObject;
            canInteract = true;
        }

        if (obj.gameObject.tag == "Generator")
        {
            interactable = obj.gameObject;
            canInteract = true;
        }

        if (obj.gameObject.tag == "Tools")
        {
            interactable = obj.gameObject;
            canInteract = true;
        }
        #endregion

        #region Level Specific
        if (obj.gameObject.tag == "HealthShow")
        {
            transform.GetComponent<PlayerHealth>().healthbar.SetActive(true);
            transform.GetComponent<SceneActivation>().PickupTutorialActive();
            Destroy(obj.gameObject);
        }

        if (obj.gameObject.tag == "HouseDoorActive")
        {
            _SceneManager.GetComponent<DialogueManager>().UpdateQuestLog("Watch TV");
            _GameSaveData._isHouseOpen = true;
            Destroy(obj.gameObject);
        }

        if (obj.gameObject.tag == "Lvl1Start")
        {
            _GameSaveData._hasCompletedPrologue = true;
            _GameSaveData._currentCutscene = 2;
            sceneTransitioner.SetActive(true);
            sceneTransitioner.GetComponent<LevelTransition>().FadeToLevel(2);
        }

        if (obj.gameObject.tag == "Lvl2Start")
        {
            sceneTransitioner.SetActive(true);
            sceneTransitioner.GetComponent<LevelTransition>().FadeToLevel(3);
        }

        if (obj.gameObject.tag == "Lvl3Start")
        {
            sceneTransitioner.SetActive(true);
            sceneTransitioner.GetComponent<LevelTransition>().FadeToLevel(4);
        }

        if (obj.gameObject.tag == "creditsStart")
        {
            sceneTransitioner.SetActive(true);
            sceneTransitioner.GetComponent<LevelTransition>().FadeToLevel(5);
        }
        #endregion
        
        if (obj.gameObject.tag == "Sludge")
        {
            transform.GetComponent<PlayerHealth>().TakeDamage();
        }
    }

    private void OnTriggerExit2D(Collider2D obj)
    {
        #region Disable Interactable
        if (obj.gameObject.tag == "FlashlightPickup")
        {
            canInteract = false;
        }

        if (obj.gameObject.tag == "StringLightsPickup")
        {
            canInteract = false;
        }

        if (obj.gameObject.tag == "PlaceStringLight")
        {
            canInteract = false;
        }
        
        if (obj.gameObject.tag == "PlaceSpecialSL")
        {
            canInteract = false;
        }

        if (obj.gameObject.tag == "Lantern")
        {
            canInteract = false;
        }
        
        if (obj.gameObject.tag == "FlameTool")
        {
            canInteract = false;
        }

        if (obj.gameObject.tag == "Generator")
        {
            canInteract = false;
        }

        if (obj.gameObject.tag == "Tools")
        {
            canInteract = false;
        }
        #endregion
    }

    #region Action Text
    private void UpdateActionTextp(string pickup) //Update when pickup
    {
        actionText.text = "You picked up <color=red>" + pickup + "</color>.";
        actionText.color = new Color(1, 1, 1, 1);
        StartCoroutine(Wait(10));
        StartCoroutine(FadeText(5));
    }

    private void UpdateActionTextn(string need) //Update when you need something before an action
    {
        actionText.text = "You need <color=red>" + need + "</color>.";
        actionText.color = new Color(1, 1, 1, 1);
        StartCoroutine(Wait(10));
        StartCoroutine(FadeText(5));
    }    

    public IEnumerator FadeText(float t)
    {
        actionText.color = new Color(actionText.color.r, actionText.color.g, actionText.color.b, 1);
        while (actionText.color.a > 0.0f)
        {
            actionText.color = new Color(actionText.color.r, actionText.color.g, actionText.color.b, actionText.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    private IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        yield return null;
    }
    #endregion

    #region Travel Coordinates
    private IEnumerator ToHouse()
    {
        theCamera.GetComponent<CinemachineBrain>().enabled = false;
        rb2d.transform.position = new Vector3(-33.43f, -45.19f, 0);
        walkSound.clip = sounds[3];

        yield return new WaitForSeconds(0.4f);
        theCamera.SetActive(false);
        houseCamera.SetActive(true);
        flashlight.GetComponent<FollowMouse>().currentCamera = houseCamera.GetComponent<Camera>();
        FlameTool.GetComponent<FollowMouse>().currentCamera = houseCamera.GetComponent<Camera>();
        yield return null;
    }

    private IEnumerator ToBarn()
    {
        theCamera.GetComponent<CinemachineBrain>().enabled = false;
        rb2d.transform.position = new Vector3(12.99f, -45.19f, 0);
        walkSound.clip = sounds[3];

        yield return new WaitForSeconds(0.4f);
        theCamera.SetActive(false);
        barnCamera.SetActive(true);
        flashlight.GetComponent<FollowMouse>().currentCamera = barnCamera.GetComponent<Camera>();
        FlameTool.GetComponent<FollowMouse>().currentCamera = barnCamera.GetComponent<Camera>();
        yield return null;
    }

    private IEnumerator ToShed()
    {
        theCamera.GetComponent<CinemachineBrain>().enabled = false;
        rb2d.transform.position = new Vector3(6.57f, -60.56f, 0);
        walkSound.clip = sounds[3];

        yield return new WaitForSeconds(0.4f);
        theCamera.SetActive(false);
        shedCamera.SetActive(true);
        flashlight.GetComponent<FollowMouse>().currentCamera = shedCamera.GetComponent<Camera>();
        FlameTool.GetComponent<FollowMouse>().currentCamera = shedCamera.GetComponent<Camera>();
        yield return null;
    }

    private IEnumerator ToUpstairs()
    {
        rb2d.transform.position = new Vector3(-17.61f, -56.55f, 0);

        yield return new WaitForSeconds(0.4f);
        houseCamera.transform.position = new Vector3(-24.66f, -58.42f, -10);
        yield return null;
    }

    private IEnumerator ToDownstairs()
    {
        rb2d.transform.position = new Vector3(-18.86f, -38.32f, 0);

        yield return new WaitForSeconds(0.4f);
        houseCamera.transform.position = new Vector3(-24.66f, -43.5f, -10f);
        yield return null;
    }

    private IEnumerator ExitHouse()
    {
        theCamera.GetComponent<CinemachineBrain>().enabled = true;
        rb2d.transform.position = new Vector3(1.92f, 0.77f, 0);

        yield return new WaitForSeconds(0.4f);
        theCamera.SetActive(true);
        houseCamera.SetActive(false);
        flashlight.GetComponent<FollowMouse>().currentCamera = theCamera.GetComponent<Camera>();
        FlameTool.GetComponent<FollowMouse>().currentCamera = theCamera.GetComponent<Camera>();
        yield return null;
    }

    private IEnumerator ExitBarn()
    {
        theCamera.GetComponent<CinemachineBrain>().enabled = true;
        rb2d.transform.position = new Vector3(-18.92f, -22.92f, 0);

        yield return new WaitForSeconds(0.4f);
        theCamera.SetActive(true);
        barnCamera.SetActive(false);
        flashlight.GetComponent<FollowMouse>().currentCamera = theCamera.GetComponent<Camera>();
        FlameTool.GetComponent<FollowMouse>().currentCamera = theCamera.GetComponent<Camera>();
        yield return null;
    }

    private IEnumerator ExitShed()
    {
        theCamera.GetComponent<CinemachineBrain>().enabled = true;
        rb2d.transform.position = new Vector3(-20.39f, 13.54f, 0);

        yield return new WaitForSeconds(0.4f);
        theCamera.SetActive(true);
        shedCamera.SetActive(false);
        flashlight.GetComponent<FollowMouse>().currentCamera = theCamera.GetComponent<Camera>();
        FlameTool.GetComponent<FollowMouse>().currentCamera = theCamera.GetComponent<Camera>();
        yield return null;
    }

    #endregion
}