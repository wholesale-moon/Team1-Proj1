using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCan : MonoBehaviour
{
    [SerializeField] private GameObject fire;
    
    private void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.gameObject.tag == "Damage")
        {
            fire.SetActive(true);
            Invoke("DelayedDisable", 0.4f);
        }
    }

    private void DelayedDisable()
    {
        Destroy(transform.gameObject);
    }
}
