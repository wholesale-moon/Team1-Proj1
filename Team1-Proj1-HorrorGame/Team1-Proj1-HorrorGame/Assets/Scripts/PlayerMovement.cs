using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float moveSpeed = 5;
    public Rigidbody2D rb2d;
    private Vector2 moveInput;
    int health = 100;
    public GameObject theCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        rb2d.velocity = moveInput * moveSpeed;
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
            theCamera.transform.position = new Vector3(-2.5f, -24.5f, -10);
            rb2d.transform.position = new Vector3(-2.5f, -26.7f, -2);
        }
        if (collision.gameObject.tag == "Barn")
        {//x 18.5  y -24.5
            theCamera.transform.position = new Vector3(18.5f, -24.5f, -10);
            rb2d.transform.position = new Vector3(18.7f, -26.7f, -2);
        }
        if (collision.gameObject.tag == "Field")
        {
            theCamera.transform.position = new Vector3(0f, 0f, -10);
            rb2d.transform.position = new Vector3(4.5f, 1.8f, -2);
        }
        if (collision.gameObject.tag == "Field2")
        {
            theCamera.transform.position = new Vector3(0f, 0f, -10);
            rb2d.transform.position = new Vector3(-4.5f, 2.5f, -2);
        }
    }
}
