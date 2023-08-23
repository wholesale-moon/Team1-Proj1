using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Queue<string> sentences;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public GameObject DialogueBox;

    void Start()
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
    }

    void EndDialogue()
    {
        DialogueBox.SetActive(false);
    }

}
