using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float moveSpeed = 5;
    public Rigidbody2D rb2d; // you can use [SerializeField] private xxx to make it so you can see it in the inspector
    private Vector2 moveInput; // but not public for other scripts to accidentally use.
    int health = 100;
    public GameObject theCamera;
    public GameObject houseCamera;
    public AudioSource walkSound;
    public AudioSource fieldTheme;
    public AudioSource houseTheme;
    bool isMoving = false;

    //Animations stuff
    [SerializeField] private Animator animator;

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
            rb2d.transform.position = new Vector3(-2.45f, -43.15f, 0);
            fieldTheme.Stop();
            houseTheme.Play();
        }
        if (collision.gameObject.tag == "Barn")
        {//x 18.5  y -24.5
            theCamera.transform.position = new Vector3(18.5f, -24.5f, -10);
            rb2d.transform.position = new Vector3(18.7f, -26.7f, -2);
        }
        if (collision.gameObject.tag == "Field")
        {
            theCamera.SetActive(true);
            houseCamera.SetActive(false);
            rb2d.transform.position = new Vector3(4.5f, 1.8f, -2);
            houseTheme.Stop();
            fieldTheme.Play();
        }
        if (collision.gameObject.tag == "Field2")
        {
            theCamera.transform.position = new Vector3(0f, 0f, -10);
            rb2d.transform.position = new Vector3(-4.5f, 2.5f, -2);
        }
        if (collision.gameObject.tag == "Flashlight")
        {
            Destroy(collision.gameObject);
            Debug.Log("You picked up a flashlight!");
        }
    }
}
