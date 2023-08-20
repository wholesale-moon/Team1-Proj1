using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float moveSpeed = 5;
    public Rigidbody2D rb2d;
    private Vector2 moveInput;
    int health = 100;

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
        {
            Debug.Log("House is not enterable yet");
        }
        if (collision.gameObject.tag == "Barn")
        {
            Debug.Log("Barn is not enterable yet");
        }
    }
}
