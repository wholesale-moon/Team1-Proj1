using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject DialogueBox;

    [Header("Settings")]
    [SerializeField] private bool isOneTime;
    [SerializeField] private bool isOnAwake;
    [SerializeField] private bool isOnEnable;
    [SerializeField] private bool isOnTriggerEnter;

    [Header("Dialogue")]
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    void Awake()
    {
        if (isOnAwake)
        {
            TriggerDialogue();
                
            if (isOneTime)
            {
                Destroy(gameObject);
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D obj)
    {
        if(isOnTriggerEnter)
        {
            if(obj.gameObject.tag == "Player")
            {
                TriggerDialogue();
                
                if (isOneTime)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    void OnEnable()
    {
        if (isOnEnable)
        {
            TriggerDialogue();
                
            if (isOneTime)
            {
                Destroy(gameObject);
            }
        }
    }
}
