using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    public Rigidbody2D rb2d;
    [SerializeField] private Animator animator;
    private Vector2 moveInput;
    float moveSpeed = 5;
    int health = 100;

    [Header("Action Text")]
    [SerializeField] private TMP_Text actionText;

    [Header("Cameras")]
    public GameObject theCamera;
    public GameObject houseCamera;

    [Header("Audio")]
    public AudioSource walkSound;
    public AudioSource speaker;
    public AudioClip[] audio;
    bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        walkSound = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        OnMove();        
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
        if (collision.gameObject.tag == "Scarecrow")
        {
            health -= 10;
            Debug.Log("You have taken 10 damage!");
            if (health <= 0)
            {
                health = 0;
                Debug.Log("You have lost all of your health!");
            }
        }
        if (collision.gameObject.tag == "House")
        { //X -2.5  y -24.5
            theCamera.SetActive(false);
            houseCamera.SetActive(true);
            rb2d.transform.position = new Vector3(-8.2f, -41.14f, 0);
            speaker.clip = audio[1];
            //walkSound.clip = audio[?];
            speaker.Play();
        }
        if (collision.gameObject.tag == "Barn")
        {//x 18.5  y -24.5
            theCamera.transform.position = new Vector3(18.5f, -24.5f, -10);
            rb2d.transform.position = new Vector3(18.7f, -26.7f, -2);
            speaker.clip = audio[1];
            speaker.Play();
        }
        if (collision.gameObject.tag == "Field")
        {
            theCamera.SetActive(true);
            houseCamera.SetActive(false);
            rb2d.transform.position = new Vector3(4.5f, 1.8f, -2);
            speaker.clip = audio[0];
            speaker.Play();
        }
        if (collision.gameObject.tag == "Field2")
        {
            theCamera.transform.position = new Vector3(0f, 0f, -10);
            rb2d.transform.position = new Vector3(-4.5f, 2.5f, -2);
            speaker.clip = audio[0];
            speaker.Play();
        }
        if (collision.gameObject.tag == "Flashlight")
        {
            Destroy(collision.gameObject);
            UpdateActionText("Flashlight");
        }
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "Corn")
        {
            walkSound.clip = audio[3];
        }

        if (obj.gameObject.tag == "Grass")
        {
            walkSound.clip = audio[2];
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
    }

    private void UpdateActionText(string pickup)
    {
        actionText.text = "You picked up <color=red>" + pickup + "</color>.";
        actionText.color = new Color(1,1,1,1);
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
