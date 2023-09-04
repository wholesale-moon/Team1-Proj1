using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Menu Objects")]
    [SerializeField] private GameObject PauseScreen;
    [SerializeField] private GameObject HelpScreen;
    [SerializeField] private GameObject OptionsScreen;

    public bool isPaused = false;
    public bool isHelp = false;
    public bool isOptions = false;

    [SerializeField] private GameObject sceneTransitioner;

    private void Update()
    {
        if(Input.GetKeyDown("escape"))
        {
            isPaused = !isPaused;
            if(isHelp & isPaused)
            {
                isHelp = false;
                HelpScreen.SetActive(isHelp);
            } else if (isOptions & isPaused)
            {
                isOptions = false;
                OptionsScreen.SetActive(isOptions);
            }

            PauseScreen.SetActive(isPaused);
        }

        if (isPaused || isHelp || isOptions)
        {
            Time.timeScale = 0.0f;
        } else if (!isPaused && !isHelp && !isOptions){
            Time.timeScale = 1.0f;
        }
    }

    public void OnButtonHighlight()
    {
        gameObject.GetComponent<SoundManager>().PlayClipByName("Button Highlight");
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

    public void Restart()
    {
        sceneTransitioner.SetActive(true);
        sceneTransitioner.GetComponent<LevelTransition>().FadeToLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void Help()
    {
        isPaused = false;
        PauseScreen.SetActive(isPaused);
        
        isHelp = true;
        HelpScreen.SetActive(isHelp);

        isOptions = false;
        OptionsScreen.SetActive(isOptions);
    }

    public void Options()
    {
        isPaused = false;
        PauseScreen.SetActive(isPaused);
        
        isHelp = false;
        HelpScreen.SetActive(isHelp);
        
        isOptions = true;
        OptionsScreen.SetActive(isOptions);
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
