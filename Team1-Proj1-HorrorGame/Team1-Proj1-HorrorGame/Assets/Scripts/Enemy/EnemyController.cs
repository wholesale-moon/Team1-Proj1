using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private GameSaveData _GameSaveData;
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float roamRadius = 5f;
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D rb;

    private Vector2 roamPosition;
    public bool IsInRange = false;
    private bool isDead = false;
    public bool isRoaming = true;
    public bool isChasing = false;

    [Header("Stun")]
    [SerializeField] private int stunTime;
    public bool isStunned;

    [Header("Animation")]
    [SerializeField] private Animator animatorTop;
    [SerializeField] private Animator animatorBottom;

    [Header("Audio")]
    [SerializeField] private GameObject SceneManager;
    public AudioSource EnemySound;
    public AudioClip ChaseSound;
    public AudioClip[] RoamSound;


    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }
    
    private void Start()
    {        
        rb = GetComponent<Rigidbody2D>();
        
        roamPosition = GetRandomRoamPosition();
    }

    private void Update()
    {        
        if (isDead)
            return;
        
        if (isStunned)
        {
            SceneManager.GetComponent<SoundManager>().PlayClipByName("EnemyGrowl");
            animatorTop.SetFloat("Speed", 0);
            animatorBottom.SetFloat("Speed", 0);
            rb.velocity = Vector2.zero;
            return;
        }
        else if (!isStunned)
        {
            animatorTop.SetFloat("Speed", 1);
            animatorBottom.SetFloat("Speed", 1);
            if (IsInRange)
            {
                animatorTop.SetBool("isHostile", true);
                animatorBottom.SetBool("isHostile", true);
                ChasePlayer();
            }
            else if (!IsInRange)
            {
                animatorTop.SetBool("isHostile", false);
                animatorBottom.SetBool("isHostile", false);
                Roam();
                isChasing = false;
            }
        }         
    }

    AudioClip RandomClip()
    {
        return RoamSound[Random.Range(0, RoamSound.Length)];
    }

    public void AnimateMovement(Vector3 direction)
    {
        animatorTop.SetFloat("Horizontal", direction.x);
        animatorTop.SetFloat("Vertical", direction.y);
        animatorBottom.SetFloat("Horizontal", direction.x);
        animatorBottom.SetFloat("Vertical", direction.y);

        
    }
    
    public void Roam()
    {        
        if (Vector2.Distance(transform.position, roamPosition) > 0.1f)
        {
            Vector2 direction = roamPosition - (Vector2)transform.position;
            direction.Normalize();
            rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
            AnimateMovement(direction);
            isRoaming = true;
        }
        else
        {
            roamPosition = GetRandomRoamPosition();
        }
    }
    public void ChasePlayer()
    {
        //SceneManager.GetComponent<SoundManager>().PlayClipByName("EnemyBreath");
        Vector2 direction = player.position - transform.position;
        direction.Normalize();
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
        AnimateMovement(direction);
        isChasing = true;
        isRoaming = false;
        
    }

    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "Damage")
        {
            animatorTop.SetTrigger("isBurn");
            animatorBottom.SetTrigger("isBurn");
            
            StartCoroutine(Death());
        }     
    }

    private IEnumerator Death()
    {
        SceneManager.GetComponent<SoundManager>().PlayClipByName("EnemyDeath");
        isDead = true;
        transform.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(2.7f);

        _GameSaveData._numOfScarecrows -= 1;
        Destroy(gameObject);
        yield return null;
    }
    
    void OnCollisionEnter2D(Collision2D obj)
    {
        if(obj.gameObject.tag == "Player")
        {
            obj.gameObject.GetComponent<PlayerHealth>().TakeDamage();
        }

        if(obj.gameObject.tag == "MapBorder")
        {
            roamPosition = GetRandomRoamPosition();
        }
    }
    public void Stunned()
    {
        
        isStunned = true;
        animatorTop.SetTrigger("isStunned");
        animatorBottom.SetTrigger("isStunned");
        StartCoroutine(HandleStunTime());
    }

    public IEnumerator HandleStunTime()
    {
        yield return new WaitForSeconds(stunTime);
        isStunned = false;
        roamPosition = GetRandomRoamPosition();
    }

    private Vector3 GetRandomRoamPosition()
    {
        Vector3 randomDirection = Random.insideUnitCircle.normalized;
        return transform.position + randomDirection * roamRadius;
    }
    public IEnumerator EnemyRoam()
    {
        EnemySound.PlayOneShot(RandomClip());
        yield return new WaitForSeconds(5);
    }
}
