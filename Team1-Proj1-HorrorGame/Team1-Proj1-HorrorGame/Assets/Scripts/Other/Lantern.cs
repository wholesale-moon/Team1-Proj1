using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    [SerializeField] private GameObject initialSprite;
    [SerializeField] private GameObject crowSteal;
    [SerializeField] private float stealTime = 7.0f;

    private void Awake()
    {
        StartCoroutine(Steal(stealTime));
    }

    private IEnumerator Steal(float time)
    {
        yield return new WaitForSeconds(time);
        crowSteal.SetActive(true);

        yield return new WaitForSeconds(1.33f);
        initialSprite.SetActive(false);

        yield return new WaitForSeconds(1.66f);
        Destroy(gameObject);
    }
}
