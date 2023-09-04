using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Scarecrow")
        {
            if(collision.GetComponent<EnemyMovement>().isStunned == false)
            {
                collision.gameObject.GetComponent<EnemyMovement>().Stunned();
                StartCoroutine(collision.gameObject.GetComponent<EnemyMovement>().HandleStunTime());
            }
        }
    }
}
