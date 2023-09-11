using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject PlacedGasCan;
    
    public void EnableCollider()
    {
        transform.GetComponent<Collider2D>().enabled = true;
    }

    public void DisableCollider()
    {
        transform.GetComponent<Collider2D>().enabled = false;
    }

    public void DestroyObject()
    {
        Destroy(PlacedGasCan);
    }
}
