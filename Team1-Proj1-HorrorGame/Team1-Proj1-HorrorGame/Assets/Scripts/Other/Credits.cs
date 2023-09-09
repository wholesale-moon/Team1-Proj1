using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [SerializeField] private bool isShowing = false;
    
    public void ShowText(GameObject credit)
    {
        if (isShowing == false)
        {
            isShowing = true;
            credit.SetActive(isShowing);
        } else {
            isShowing = false;
            credit.SetActive(isShowing);
        }
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
