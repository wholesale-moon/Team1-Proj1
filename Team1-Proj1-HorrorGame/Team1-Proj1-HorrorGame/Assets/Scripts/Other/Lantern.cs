using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    [SerializeField] private float stealTime = 7.0f;

    private void Awake()
    {
        StartCoroutine(Steal(stealTime));
    }

    private IEnumerator Steal(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
}
