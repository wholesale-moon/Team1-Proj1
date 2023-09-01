using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{ 
    public Camera currentCamera;
    
    void Update()
    {
        faceMouse();
    }

    void faceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = currentCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

        transform.right = direction;
    }
}
