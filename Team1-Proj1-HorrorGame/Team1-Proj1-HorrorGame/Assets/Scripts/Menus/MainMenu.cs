using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject _SceneManager;
    [SerializeField] AudioMixerGroup musicMixerGroup;

    [SerializeField] private GameObject optionsScreen;
    
    public void OnButtonHighlight()
    {
        _SceneManager.GetComponent<SoundManager>().PlayClipByName("Button Highlight");
    }
    
    public void PlayGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(1);
    }

    public void OptionsOpen()
    {
        optionsScreen.SetActive(true);
    }

    public void OptionsClose()
    {
        optionsScreen.SetActive(false);
    }

    public void Tutorial()
    {
        SceneManager.LoadScene(4);
    }
    
    public void Credits()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
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
