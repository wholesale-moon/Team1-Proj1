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
    [Header("Player Movement")]
    public Rigidbody2D rb2d;
    [SerializeField] private Animator animator;
    private Vector2 moveInput;
    float moveSpeed = 5;

    private Vector3 outsideScale = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 insideScale = new Vector3(0.65f, 0.65f, 0.65f);

    [Header("Health")]
    [SerializeField] int health = 4;
    [SerializeField] int numOfHearts = 4;
    [SerializeField] Image[] hearts;

    [Header("Action Text")]
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
    [SerializeField] private GameObject _SceneManager;
    public GameObject flashlight;

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

        if (health > numOfHearts)
        {
            health = numOfHearts;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health && i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*if (collision.gameObject.tag == "Scarecrow")
        {
            health -= 10;
            Debug.Log("You have taken 10 damage!");
            if (health <= 0)
            {
                health = 0;
                Debug.Log("You have lost all of your health!");
            }
        }*/
        
        if (collision.gameObject.tag == "House")
        {
            StartCoroutine(ToHouse());
        }
        
        if (collision.gameObject.tag == "Barn")
        {
            StartCoroutine(ToBarn());
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
            StartCoroutine(ExitBarn());
        }

        if (collision.gameObject.tag == "Field3")
        {
            StartCoroutine(ExitShed());
        }
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
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

        if (obj.gameObject.tag == "Flashlight")
        {
            Destroy(obj.gameObject);
            UpdateActionText("Flashlight");
        }

        if (obj.gameObject.tag == "Medicine")
        {
            // if (health != max)
            // {
            //     Destroy(obj.gameObject);
            //     UpdateActionText("Medicine");
            // }
            Destroy(obj.gameObject);
            UpdateActionText("Medicine");
        }

        if (obj.gameObject.tag == "Sludge")
        {
            health -= 1;
            numOfHearts -= 1;
            if (health <= 0 && numOfHearts <= 0)
            {
                health = 0;
                numOfHearts = 0;
                Debug.Log("You have lost all of your health!");
            }
        }
        // if (obj.gameObject.tag == "TV")
        // {

        //     SceneManager.LoadScene(2);
        //     walkSound.clip = sounds[3];

        // }
    }

    private void UpdateActionText(string pickup)
    {
        actionText.text = "You picked up <color=red>" + pickup + "</color>.";
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

#region Travel
    private IEnumerator ToHouse()
    {
        theCamera.GetComponent<CinemachineBrain>().enabled = false;
        rb2d.transform.position = new Vector3(-8.2f, -41.14f, 0);
        _SceneManager.GetComponent<SoundManager>().PlayClipByName("House Theme");
        walkSound.clip = sounds[3];
        transform.localScale = insideScale;

        yield return new WaitForSeconds(0.4f);
        theCamera.SetActive(false);
        houseCamera.SetActive(true);
        yield return null;
    }

    private IEnumerator ToBarn()
    {
        theCamera.GetComponent<CinemachineBrain>().enabled = false;
        rb2d.transform.position = new Vector3(39.96f, -41.76f, -2);
        _SceneManager.GetComponent<SoundManager>().PlayClipByName("House Theme");
        walkSound.clip = sounds[3];
        transform.localScale = insideScale;

        yield return new WaitForSeconds(0.4f);
        theCamera.SetActive(false);
        barnCamera.SetActive(true);
    }

    private IEnumerator ToShed()
    {
        theCamera.GetComponent<CinemachineBrain>().enabled = false;
        rb2d.transform.position = new Vector3(35.58f, -56.99f, -2);
        _SceneManager.GetComponent<SoundManager>().PlayClipByName("House Theme");
        walkSound.clip = sounds[3];
        transform.localScale = insideScale;

        yield return new WaitForSeconds(0.4f);
        theCamera.SetActive(false);
        shedCamera.SetActive(true);
    }

    private IEnumerator ExitHouse()
    {
        theCamera.GetComponent<CinemachineBrain>().enabled = true;
        rb2d.transform.position = new Vector3(4.5f, 1.8f, -2);
        _SceneManager.GetComponent<SoundManager>().PlayClipByName("Field Theme");
        transform.localScale = outsideScale;

        yield return new WaitForSeconds(0.4f);
        theCamera.SetActive(true);
        houseCamera.SetActive(false);
    }

    private IEnumerator ExitBarn()
    {
        theCamera.GetComponent<CinemachineBrain>().enabled = true;
        theCamera.transform.position = new Vector3(0f, 0f, -10);
        rb2d.transform.position = new Vector3(-4.5f, 2.5f, -2);
        _SceneManager.GetComponent<SoundManager>().PlayClipByName("Field Theme");
        transform.localScale = outsideScale;

        yield return new WaitForSeconds(0.4f);
        theCamera.SetActive(true);
        barnCamera.SetActive(false);
    }

    private IEnumerator ExitShed()
    {
        theCamera.GetComponent<CinemachineBrain>().enabled = true;
        theCamera.transform.position = new Vector3(0f, 0f, -10);
        rb2d.transform.position = new Vector3(-49.55f, 0.07f, -2);
        _SceneManager.GetComponent<SoundManager>().PlayClipByName("Field Theme");
        transform.localScale = outsideScale;

        yield return new WaitForSeconds(0.4f);
        theCamera.SetActive(true);
        shedCamera.SetActive(false);
    }

#endregion
}
