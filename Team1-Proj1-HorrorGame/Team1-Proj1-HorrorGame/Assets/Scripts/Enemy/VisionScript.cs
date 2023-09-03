using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionScript : MonoBehaviour
{
    
    [SerializeField] private GameObject scarecrow;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "Player")
        {
            scarecrow.GetComponent<EnemyMovement>().IsInRange = true;
        }
    }

}
