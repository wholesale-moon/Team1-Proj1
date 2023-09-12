using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameSaveData _GameSaveData;
    
    [Header("Menu Objects")]
    [SerializeField] private GameObject PauseScreen;
    [SerializeField] private GameObject HelpScreen;
    [SerializeField] private GameObject OptionsScreen;

    [HideInInspector] public bool isPaused = false;
    [HideInInspector] public bool isHelp = false;
    [HideInInspector] public bool isOptions = false;

    [SerializeField] private GameObject sceneTransitioner;
    
    [Space(10)]
    [SerializeField] private GameObject Flashlight;
    [SerializeField] private GameObject FlameTool;

    private void Start()
    {
        _GameSaveData._gamePause = false;
    }
    
    private void Update()
    {
        if (_GameSaveData._gamePause)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }

        
        if(Input.GetKeyDown("escape") || Input.GetButtonDown("Controller Start"))
        {
            if (_GameSaveData._gamePause == false)
            {
                PauseScreen.SetActive(true);
                isPaused = true;
                Time.timeScale = 0.0f;
                Flashlight.GetComponent<FollowMouse>().enabled = false;
                FlameTool.GetComponent<FollowMouse>().enabled = false;

                _GameSaveData._gamePause = true;
            } else if (_GameSaveData._gamePause == true)
            {
                PauseScreen.SetActive(false);
                isPaused = false;

                OptionsScreen.SetActive(false);
                isOptions = false;

                HelpScreen.SetActive(false);
                isHelp = false;

                Time.timeScale = 1.0f;
                Flashlight.GetComponent<FollowMouse>().enabled = true;
                FlameTool.GetComponent<FollowMouse>().enabled = true;
                _GameSaveData._gamePause = false;
            }
        }
        
        
        // if(Input.GetKeyDown("escape") || Input.GetButtonDown("Controller Start"))
        // {
        //     isPaused = !isPaused;

        //     if(isHelp & isPaused)
        //     {
        //         isHelp = false;
        //         HelpScreen.SetActive(isHelp);
        //     } else if (isOptions & isPaused)
        //     {
        //         isOptions = false;
        //         OptionsScreen.SetActive(isOptions);
        //     }

        //     PauseScreen.SetActive(isPaused);
        // }

        // if (isPaused || isHelp || isOptions)
        // {
        //     Time.timeScale = 0.0f;
        //     Flashlight.GetComponent<FollowMouse>().enabled = false;
        //     FlameTool.GetComponent<FollowMouse>().enabled = false;
        // } else if (!isPaused && !isHelp && !isOptions){
            // Time.timeScale = 1.0f;
            // Cursor.visible = false;
            // Flashlight.GetComponent<FollowMouse>().enabled = true;
            // FlameTool.GetComponent<FollowMouse>().enabled = true;
        // }
        
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
        Time.timeScale = 1.0f;
        Flashlight.GetComponent<FollowMouse>().enabled = true;
        FlameTool.GetComponent<FollowMouse>().enabled = true;
        _GameSaveData._gamePause = false;
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

        Cursor.visible = true;
    }

    public void Options()
    {
        isPaused = false;
        PauseScreen.SetActive(isPaused);
        
        isHelp = false;
        HelpScreen.SetActive(isHelp);
        
        isOptions = true;
        OptionsScreen.SetActive(isOptions);

        Cursor.visible = true;
    }

    public void Back()
    {
        isPaused = true;
        PauseScreen.SetActive(isPaused);
        
        isHelp = false;
        HelpScreen.SetActive(isHelp);

        isOptions = false;
        OptionsScreen.SetActive(isOptions);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
