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
    public AudioClip DeathSound;
    public AudioClip StunSound;
    public AudioClip[] ChaseSound;
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

    private void FixedUpdate()
    {        
        if (isDead)
            return;
        
        if (isStunned)
        {
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
            }
        }
    }

    void LateUpdate()
    {
        if (SceneManager.GetComponent<UIManager>().isPaused || SceneManager.GetComponent<UIManager>().isHelp || SceneManager.GetComponent<UIManager>().isOptions)
        {
            EnemySound.Pause();
            return;
        } else if (SceneManager.GetComponent<UIManager>().isPaused == false & SceneManager.GetComponent<UIManager>().isHelp == false & SceneManager.GetComponent<UIManager>().isOptions == false)
        {
            EnemySound.UnPause(); 
        }
        
        if (!EnemySound.isPlaying)
        {
            if (isChasing)
            {
                EnemySound.clip = RandomClip(1);
                EnemySound.Play();
            } 
            else if (isRoaming)
            {
                EnemySound.clip = RandomClip(0);
                EnemySound.Play();
            }
            else if (isStunned)
            {
                EnemySound.Stop();
                EnemySound.clip = StunSound;
                EnemySound.Play();
            }
        }
    }

    AudioClip RandomClip(int num)
    {
        switch (num)
        {
            case 0:
                return RoamSound[Random.Range(0, RoamSound.Length)];
                break;
            case 1:
                return ChaseSound[Random.Range(0, RoamSound.Length)];
                break;
            default:
                Debug.Log(num + " is not a valid case number.");
                return null;
                break;
        }
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
        EnemySound.Stop();
        EnemySound.clip = DeathSound;
        EnemySound.Play();
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

    public IEnumerator ExitChaseCooldown()
    {
        yield return new WaitForSeconds(1);

        isChasing = false;
        yield return null;
    }

    private Vector3 GetRandomRoamPosition()
    {
        Vector3 randomDirection = Random.insideUnitCircle.normalized;
        return transform.position + randomDirection * roamRadius;
    }
}
