using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject _SceneManager;
    [SerializeField] AudioMixerGroup musicMixerGroup;

    [SerializeField] GameSaveData _GameSaveData;
    [SerializeField] private GameObject optionsScreen;
    [SerializeField] private GameObject skipPrologueOption;
    
    void Start()
    {
        if(_GameSaveData._hasCompletedPrologue == true)
        {
            skipPrologueOption.SetActive(true);
        }
    }
    
    public void OnButtonHighlight()
    {
        _SceneManager.GetComponent<SoundManager>().PlayClipByName("Button Highlight");
    }
    
    public void PlayGame()
    {
        if (_GameSaveData._skipPrologue == true)
        {
            SceneManager.LoadScene(2);
        } else {
            SceneManager.LoadScene(1);
        }
    }

    public void OptionsOpen()
    {
        optionsScreen.SetActive(true);
    }

    public void OptionsClose()
    {
        optionsScreen.SetActive(false);
    }
    
    public void Credits()
    {
        SceneManager.LoadScene(5);
    }
    
    public void BackButton()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
