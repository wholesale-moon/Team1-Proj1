using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    public Rigidbody2D rb2d;
    [SerializeField] private Animator animator;
    private Vector2 moveInput;
    float moveSpeed = 5;
    [SerializeField] int health = 4;
    [SerializeField] int numOfHearts = 4;
    [SerializeField] Image[] hearts;

    [Header("Action Text")]
    [SerializeField] private TMP_Text actionText;

    [Header("Cameras")]
    public GameObject theCamera;
    public GameObject houseCamera;
    public GameObject barnCamera;

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
        { //X -2.5  y -24.5
            theCamera.SetActive(false);
            houseCamera.SetActive(true);
            rb2d.transform.position = new Vector3(-8.2f, -41.14f, 0);
            _SceneManager.GetComponent<SoundManager>().PlayClipByName("House Theme");
            walkSound.clip = sounds[3];
        }
        
        if (collision.gameObject.tag == "Barn")
        {//x 18.5  y -24.5
            theCamera.transform.position = new Vector3(39.96f, -41.76f, -10);
            barnCamera.SetActive(true);
            rb2d.transform.position = new Vector3(39.96f, -41.76f, -2);
            _SceneManager.GetComponent<SoundManager>().PlayClipByName("House Theme");
            walkSound.clip = sounds[3];
        }
        
        if (collision.gameObject.tag == "Field")
        {
            theCamera.SetActive(true);
            houseCamera.SetActive(false);
            rb2d.transform.position = new Vector3(4.5f, 1.8f, -2);
            _SceneManager.GetComponent<SoundManager>().PlayClipByName("Field Theme");
        }
        
        if (collision.gameObject.tag == "Field2")
        {
            barnCamera.SetActive(false);
            theCamera.transform.position = new Vector3(0f, 0f, -10);
            rb2d.transform.position = new Vector3(-4.5f, 2.5f, -2);
            _SceneManager.GetComponent<SoundManager>().PlayClipByName("Field Theme");
        }
        
        // if (collision.gameObject.tag == "Flashlight")
        // {
        //     Destroy(collision.gameObject);
        //     flashlight.SetActive(true);
        //     UpdateActionText("Flashlight");
        // }
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
            // if (health = max)
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
        if (obj.gameObject.tag == "TV")
        {

            SceneManager.LoadScene(2);
            walkSound.clip = sounds[3];

        }
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
}
