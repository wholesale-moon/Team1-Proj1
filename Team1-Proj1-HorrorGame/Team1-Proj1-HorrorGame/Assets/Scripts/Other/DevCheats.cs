using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevCheats : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Keypad0) & Input.GetKey(KeyCode.LeftControl))
        {
            SceneManager.LoadScene(0);
        }
        else if (Input.GetKey(KeyCode.Keypad1) & Input.GetKey(KeyCode.LeftControl))
        {
            SceneManager.LoadScene(1);
        }
        else if (Input.GetKey(KeyCode.Keypad2) & Input.GetKey(KeyCode.LeftControl))
        {
            SceneManager.LoadScene(2);
        }
        else if (Input.GetKey(KeyCode.Keypad3) & Input.GetKey(KeyCode.LeftControl))
        {
            SceneManager.LoadScene(3);
        }
        else if (Input.GetKey(KeyCode.Keypad4) & Input.GetKey(KeyCode.LeftControl))
        {
            SceneManager.LoadScene(4);
        }
        else if (Input.GetKey(KeyCode.Keypad5) & Input.GetKey(KeyCode.LeftControl))
        {
            SceneManager.LoadScene(5);
        }
    }
}
