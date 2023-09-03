using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Scene Manager")]
    [SerializeField] private GameObject SceneManager;
    
    [Header("Dialogue")]
    public Dialogue dialogue;

    [Header("Settings")]
    [SerializeField] private bool isOneTime;

    [Space(10)]
    [SerializeField] private bool isOnAwake;
    [SerializeField] private bool isOnEnable;
    [SerializeField] private bool isOnTriggerEnter;

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

    void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.gameObject.tag == "Player")
        {
            if(isOnTriggerEnter)
            {
                TriggerDialogue();
                if (isOneTime)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
