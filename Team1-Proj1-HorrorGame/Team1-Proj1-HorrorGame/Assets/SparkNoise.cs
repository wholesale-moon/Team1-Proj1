using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkNoise : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    public void PlaySparkNoise()
    {
        source.Play();
    }
}
