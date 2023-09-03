using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float roamRadius = 5f;
    public Transform player;

    private Vector3 roamPosition;

    public bool IsInRange = false;
    public int stunTime;
    public bool isStunned;


    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }
    private void Start()
    {
        roamPosition = GetRandomRoamPosition();
    }

    private void Update()
    {
        if (IsInRange)
        {
            ChasePlayer();
        }
        else if (!IsInRange)
        {
            Roam();
        }
        Debug.Log(isStunned);
    }
    
    public void Roam()
    {
        if (Vector3.Distance(transform.position, roamPosition) > 0.1f)
        {
            Vector3 direction = roamPosition - transform.position;
            direction.Normalize();
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else
        {
            roamPosition = GetRandomRoamPosition();
        }
    }

    public void ChasePlayer()
    {
        Vector3 direction = player.position -transform.position;
        direction.Normalize();
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            IsInRange = true;
        }
        
    }

    void OnTriggerExit2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            IsInRange = false;
            roamPosition = GetRandomRoamPosition();
        } 
        
    }
    public void Stunned()
    {
        isStunned = true;
        StartCoroutine(HandleStunTime());
    }

    public IEnumerator HandleStunTime()
    {
        yield return new WaitForSeconds(stunTime);
        isStunned = false;
        //GetComponent<AudioSource>().Play();
    }


    private Vector3 GetRandomRoamPosition()
    {
        Vector3 randomDirection = Random.insideUnitCircle.normalized;
        return transform.position + randomDirection * roamRadius;
    }
}




/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject player;
    public float speed;
    private float distance;
    public GameObject scarecrows;

    //public Rigidbody2D rb2d;
    //private Vector2 moveInput;
    //float moveSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        //rb2d = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //distance = Vector2.Distance(transform.position, player.transform.position);
        //Vector2 direction = player.transform.position - transform.position;
        //rb2d.velocity = moveInput * moveSpeed;


        //if (distance < 60)
        //{
            //transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerHealth>().TakeDamage();
        }
    }
}*/
