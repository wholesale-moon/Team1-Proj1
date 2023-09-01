using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
    [SerializeField] private GameObject outlines;

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            outlines.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            outlines.SetActive(false);
        }
    }
}
