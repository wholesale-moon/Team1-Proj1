using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    private Light2D _light;
    [SerializeField] private float[] flickerTime;
    private void Awake()
    {
        _light = GetComponent<Light2D>();
    }

    private void Start()
    {
        StartCoroutine(FlickerLights());
    }

    private IEnumerator FlickerLights()
    {
        while (true)
        {
            _light.enabled = false;
            yield return new WaitForSeconds(Random.Range(flickerTime[0], flickerTime[1]));
            _light.enabled = true;
            yield return new WaitForSeconds(Random.Range(flickerTime[0], flickerTime[1]));
        }
    }
}

