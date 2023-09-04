using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Menu Objects")]
    [SerializeField] private GameObject PauseScreen;
    [SerializeField] private GameObject HelpScreen;
    [SerializeField] private GameObject OptionsScreen;
    

    private bool isPaused = false;
    private bool isHelp = false;
    private bool isOptions = false;
    

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
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
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
