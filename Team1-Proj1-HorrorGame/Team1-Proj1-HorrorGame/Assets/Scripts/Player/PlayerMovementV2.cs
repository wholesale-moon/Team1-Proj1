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
    [SerializeField] private Animator animator;
    private Vector2 moveInput;
    float moveSpeed = 5;

    private bool canInteract;
    private GameObject interactable;

    [Header("HUD")]
    public TMP_Text questLog;
    [SerializeField] private GameObject itemHold;
    [SerializeField] private GameObject flashlightHold;
    [SerializeField] private GameObject lanternHold;

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
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        walkSound = gameObject.GetComponent<AudioSource>();
        walkSound.outputAudioMixerGroup = soundEffectsMixerGroup;
    }

    // Update is called once per frame
    void Update()
    {
        OnMove();
        
        CheckInteractables();
    }

    #region Player Movement
    void OnMove()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // Setting animation bools to determine which animation should play
        animator.SetFloat("Horizontal", moveInput.x);
        animator.SetFloat("Vertical", moveInput.y);
        animator.SetFloat("Speed", moveInput.sqrMagnitude);

        moveInput.Normalize();

        rb2d.velocity = moveInput * moveSpeed;


        if (rb2d.velocity.x != 0 || rb2d.velocity.y != 0)
        {

            isMoving = true;

            if (isMoving)
            {
                if (!walkSound.isPlaying)
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
                canInteract = false;
            } 
            else if (interactable.tag == "LanternStash") 
            {
                _GameSaveData._hasLantern = true;
                //flashlight.SetActive(false);
                flashlightHold.SetActive(false);
                lanternHold.SetActive(true);
                // set player lantern light on
                UpdateActionTextp("Lantern");
            }
            else if (interactable.tag == "StringLightsPickup")
            {
                Destroy(interactable);
                _GameSaveData._hasStringLights = true;
            }
            else if (interactable.tag == "Generator")
            {
                Destroy(interactable);
                transform.GetComponent<SceneActivation>().GeneratorOn();
            }
            else if (interactable.tag == "FlameTool")
            {
                //_GameSaveData._hasFlameTool = true;
                UpdateActionTextp("FlameTool");
            }
        }
    }

    #region Travel
    private void OnCollisionEnter2D(Collision2D collision)
    {   
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
                if (transform.GetComponent<SceneActivation>().hasMorphed == false)
                {
                    transform.GetComponent<SceneActivation>().Morph();
                }
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
    }
    #endregion

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
            Debug.Log("has gas can");
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
        #endregion

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
            sceneTransitioner.GetComponent<LevelTransition>().FadeToLevel(2);
        }

        if (obj.gameObject.tag == "Lvl2Start")
        {
            sceneTransitioner.GetComponent<LevelTransition>().FadeToLevel(3);
        }
        
        if (obj.gameObject.tag == "Sludge")
        {
            transform.GetComponent<PlayerHealth>().TakeDamage();
        }
    }

    private void OnTriggerExit2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "FlashlightPickup")
        {
            canInteract = false;
        }

        if (obj.gameObject.tag == "StringLightsPickup")
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

    #region Travel
    private IEnumerator ToHouse()
    {
        theCamera.GetComponent<CinemachineBrain>().enabled = false;
        rb2d.transform.position = new Vector3(-6.19f, -41.3f, -2);
        //_SceneManager.GetComponent<SoundManager>().PlayClipByName("House Theme");
        walkSound.clip = sounds[3];

        yield return new WaitForSeconds(0.4f);
        theCamera.SetActive(false);
        houseCamera.SetActive(true);
        flashlight.GetComponent<FollowMouse>().currentCamera = houseCamera.GetComponent<Camera>();
        yield return null;
    }

    private IEnumerator ToBarn()
    {
        theCamera.GetComponent<CinemachineBrain>().enabled = false;
        rb2d.transform.position = new Vector3(39.96f, -41.76f, -2);
        //_SceneManager.GetComponent<SoundManager>().PlayClipByName("House Theme");
        walkSound.clip = sounds[3];

        yield return new WaitForSeconds(0.4f);
        theCamera.SetActive(false);
        barnCamera.SetActive(true);
        flashlight.GetComponent<FollowMouse>().currentCamera = barnCamera.GetComponent<Camera>();
        yield return null;
    }

    private IEnumerator ToShed()
    {
        theCamera.GetComponent<CinemachineBrain>().enabled = false;
        rb2d.transform.position = new Vector3(35.58f, -56.99f, -2);
        //_SceneManager.GetComponent<SoundManager>().PlayClipByName("House Theme");
        walkSound.clip = sounds[3];

        yield return new WaitForSeconds(0.4f);
        theCamera.SetActive(false);
        shedCamera.SetActive(true);
        flashlight.GetComponent<FollowMouse>().currentCamera = shedCamera.GetComponent<Camera>();
        yield return null;
    }

    private IEnumerator ToUpstairs()
    { // 2.48, -12.7, -10
        rb2d.transform.position = new Vector3(6.83f, -57.71f, -2f);

        yield return new WaitForSeconds(0.4f);
        houseCamera.transform.position = new Vector3(1.791583f, -53.71448f, -10f);
        yield return null;
    }

    private IEnumerator ToDownstairs()
    { // 2.48, 1.27, -10
        rb2d.transform.position = new Vector3(5.553f, -41.489f, -2);

        yield return new WaitForSeconds(0.4f);
        houseCamera.transform.position = new Vector3(1.791583f, -39.74448f, -10);
        yield return null;
    }

    private IEnumerator ExitHouse()
    {
        theCamera.GetComponent<CinemachineBrain>().enabled = true;
        rb2d.transform.position = new Vector3(2.98f, 0.68f, -2);
        _SceneManager.GetComponent<SoundManager>().PlayClipByName("Field Theme");

        yield return new WaitForSeconds(0.4f);
        theCamera.SetActive(true);
        houseCamera.SetActive(false);
        flashlight.GetComponent<FollowMouse>().currentCamera = theCamera.GetComponent<Camera>();
        yield return null;
    }

    private IEnumerator ExitBarn()
    {
        theCamera.GetComponent<CinemachineBrain>().enabled = true;
        theCamera.transform.position = new Vector3(0f, 0f, -10);
        rb2d.transform.position = new Vector3(-35.86f, -23.43f, -2);
        //_SceneManager.GetComponent<SoundManager>().PlayClipByName("Field Theme");

        yield return new WaitForSeconds(0.4f);
        theCamera.SetActive(true);
        barnCamera.SetActive(false);
        flashlight.GetComponent<FollowMouse>().currentCamera = theCamera.GetComponent<Camera>();
        yield return null;
    }

    private IEnumerator ExitShed()
    {
        theCamera.GetComponent<CinemachineBrain>().enabled = true;
        theCamera.transform.position = new Vector3(0f, 0f, -10);
        rb2d.transform.position = new Vector3(-49.55f, 0.07f, -2);
        //_SceneManager.GetComponent<SoundManager>().PlayClipByName("Field Theme");

        yield return new WaitForSeconds(0.4f);
        theCamera.SetActive(true);
        shedCamera.SetActive(false);
        flashlight.GetComponent<FollowMouse>().currentCamera = theCamera.GetComponent<Camera>();
        yield return null;
    }

    #endregion
}