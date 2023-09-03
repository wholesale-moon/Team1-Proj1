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

    [Space(10)]
    public TMP_Text screenText;

    [Header("Quest Log")]
    public GameObject questLog;
    public TMP_Text questText;
    private string questTemp;

    [Header("Scene Manager")]
    [SerializeField] private GameObject SceneManager;

    private bool isEndCutscene;
    private bool IsScreenText;

    void Awake()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        IsScreenText = dialogue.isScreenText;

        if(!IsScreenText)
        {
            nameText.text = dialogue.name;
        }
        
        if(dialogue.quest != null)
        {
            questTemp = dialogue.quest;
        }

        isEndCutscene = dialogue.doesEndCutscene;

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
        
        if(dialogue.isQuest == true)
        {
            UpdateQuestLog(questTemp);
            return;
        } else 
        {
            DialogueBox.SetActive(true);
            DisplayNextSentence();
            //Time.timeScale = 0.0f;
        }
    }

    public void DisplayNextSentence()
    {   
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        if (IsScreenText)
        {
            screenText.gameObject.SetActive(false);
            string sentence = sentences.Dequeue();
            screenText.text = sentence;
            screenText.gameObject.SetActive(true);
        } else {
            dialogueText.gameObject.SetActive(false);
            string sentence = sentences.Dequeue();
            dialogueText.text = sentence;
            dialogueText.gameObject.SetActive(true);
        }

        if(sentences.Count == 0)
        {
            contText.text = "<< END DIALOGUE >>";
        }
    }

    void EndDialogue()
    {
        DialogueBox.SetActive(false);
        
        if (questTemp != "")
        {
            UpdateQuestLog(questTemp);
        }

        if(isEndCutscene)
        {
            SceneManager.GetComponent<CutsceneTrigger>().EndCutscene();
        }

        //Time.timeScale = 1.0f;
    }

    public void UpdateQuestLog(string quest)
    {
        questLog.gameObject.SetActive(false);
        questText.text = ">> "+ quest;
        questLog.gameObject.SetActive(true);
    }
}
