using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCan : MonoBehaviour
{
    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject burnMark;
    
    private void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.gameObject.tag == "Damage")
        {
            fire.SetActive(true);
            Invoke("DelayedDisable", 0.6f);
        }
    }

    private void DelayedDisable()
    {
        Instantiate(burnMark, transform.position, Quaternion.identity);
        Destroy(transform.gameObject);
    }
}
