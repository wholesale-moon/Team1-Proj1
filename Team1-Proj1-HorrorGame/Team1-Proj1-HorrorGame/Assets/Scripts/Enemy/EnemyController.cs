using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float roamRadius = 5f;
    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D rb;

    private Vector2 roamPosition;
    public bool IsInRange = false;

    [Header("Stun")]
    [SerializeField] private int stunTime;
    public bool isStunned;

    [Header("Animation")]
    [SerializeField] private Animator animatorTop;
    [SerializeField] private Animator animatorBottom;

    [Header("Audio")]
    [SerializeField] private AudioMixerGroup soundEffectsMixerGroup;
    [SerializeField] private AudioSource mouf; //mouth
    [SerializeField] private AudioClip[] sound;


    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mouf.outputAudioMixerGroup = soundEffectsMixerGroup;
        roamPosition = GetRandomRoamPosition();
    }

    private void Update()
    {
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
    
    public void AnimateMovement(Vector3 direction)
    {
        animatorTop.SetFloat("Horizontal", direction.x);
        animatorTop.SetFloat("Vertical", direction.y);
        animatorBottom.SetFloat("Horizontal", direction.x);
        animatorBottom.SetFloat("Vertical", direction.y);
    }
    
    public void Roam()
    {
        mouf.clip = sound[0];
        if (Vector2.Distance(transform.position, roamPosition) > 0.1f)
        {
            Vector2 direction = roamPosition - (Vector2)transform.position;
            direction.Normalize();
            rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
            AnimateMovement(direction);
        }
        else
        {
            roamPosition = GetRandomRoamPosition();
        }
    }

    public void ChasePlayer()
    {
        mouf.clip = sound[2];
        Vector2 direction = player.position - transform.position;
        direction.Normalize();
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
        AnimateMovement(direction);
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
        yield return new WaitForSeconds(2.7f);

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
        mouf.clip = sound[1];
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
}
