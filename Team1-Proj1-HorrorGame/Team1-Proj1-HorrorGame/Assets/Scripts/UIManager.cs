using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Menu Objects")]
    [SerializeField] private GameObject PauseScreen;
    [SerializeField] private GameObject HelpScreen;

    [Header("Audio")]
    [SerializeField] AudioSource speaker;
    [SerializeField] AudioClip[] audios;

    private bool isPaused = false;
    private bool isHelp = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if(isHelp & isPaused)
            {
                isHelp = !isHelp;
                HelpScreen.SetActive(isHelp);
            }

            PauseScreen.SetActive(isPaused);
        }
    }

    public void OnButtonHighlight()
    {
        speaker.clip = audios[0];
        speaker.Play();
    }

    public void OnButtonClick()
    {
        //Play sound
    }
    
    public void Resume()
    {
        isPaused = false;
        PauseScreen.SetActive(isPaused);
    }

    public void Help()
    {
        isPaused = false;
        PauseScreen.SetActive(isPaused);
        
        isHelp = true;
        HelpScreen.SetActive(isHelp);
    }

    public void Back()
    {
        isPaused = true;
        PauseScreen.SetActive(isPaused);
        
        isHelp = false;
        HelpScreen.SetActive(isHelp);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
