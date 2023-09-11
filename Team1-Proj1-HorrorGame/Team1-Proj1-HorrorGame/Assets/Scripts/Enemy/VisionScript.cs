using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionScript : MonoBehaviour
{
    [SerializeField] private GameObject SceneManager;
    [SerializeField] private GameObject scarecrow;
    public AudioClip ChaseSound;
    public AudioSource EnemySound;

    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            scarecrow.GetComponent<EnemyMovement>().IsInRange = true;
            scarecrow.GetComponent<EnemyMovement>().EnemySound.Stop();
        }
    }

    void OnTriggerExit2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            scarecrow.GetComponent<EnemyMovement>().IsInRange = false;
            StartCoroutine(scarecrow.GetComponent<EnemyMovement>().ExitChaseCooldown());
        }
    }
}
