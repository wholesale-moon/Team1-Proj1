using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue Objects")]
    public GameObject DialogueBox;
    public Queue<string> sentences;
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    [SerializeField] private TMP_Text contText;

    [Header("Scene Manager")]
    [SerializeField] private GameObject SceneManager;

    void Awake()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        nameText.text = dialogue.name;

        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        if (sentences.Count > 1)
        {
            contText.text = "CONTINUE >>";
        } else {
            contText.text = "<< END DIALOGUE >>";
        }

        DialogueBox.SetActive(true);
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {   
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        dialogueText.gameObject.SetActive(false);
        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
        dialogueText.gameObject.SetActive(true);

        if(sentences.Count == 0)
        {
            contText.text = "<< END DIALOGUE >>";
        }
    }

    void EndDialogue()
    {
        DialogueBox.SetActive(false);
        //SceneManager.GetComponent<TriggerCutscene>().EndCutscene();
    }
}
