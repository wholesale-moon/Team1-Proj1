using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{ 
    [SerializeField] private GameObject SceneManager;
    private bool canMoveFlashlight;
    public Camera currentCamera;
    
    void Update()
    {
        faceMouse();
        
        // CheckIfMove(canMoveFlashlight);
        
        // if (canMoveFlashlight)
        // {
        //     Debug.Log("Facing Mouse");
        // } 
        // else
        // {
        //     Debug.Log("Stopped");
        // }
    }

    // private bool CheckIfMove(bool canMove)
    // {
    //     bool _isPaused = SceneManager.GetComponent<UIManager>().isPaused;
    //     bool _isHelp = SceneManager.GetComponent<UIManager>().isHelp;
    //     bool _isOptions = SceneManager.GetComponent<UIManager>().isOptions;

    //     if (_isPaused || _isHelp || _isOptions)
    //     {
    //         canMove = false;
    //     } else if (!_isPaused && !_isHelp && !_isOptions) {
    //         canMove = true;
    //     }

    //     return (canMove);
    // }

    void faceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = currentCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

        transform.right = direction;
    }
}
