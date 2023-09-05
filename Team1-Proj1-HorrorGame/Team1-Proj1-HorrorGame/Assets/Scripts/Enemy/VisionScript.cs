using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionScript : MonoBehaviour
{
    [SerializeField] private GameObject SceneManager;
    [SerializeField] private GameObject scarecrow;

    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            scarecrow.GetComponent<EnemyMovement>().IsInRange = true;
            SceneManager.GetComponent<SoundManager>().PlayClipByName("EnemyBreath");
        }
    }

    void OnTriggerExit2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            scarecrow.GetComponent<EnemyMovement>().IsInRange = false;
            SceneManager.GetComponent<SoundManager>().StopClipByName("EnemyBreath");
        }
    }

}
