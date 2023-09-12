using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    private bool isShowing = false;
    
    void Start()
    {
        Cursor.visible = true;
    }

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
