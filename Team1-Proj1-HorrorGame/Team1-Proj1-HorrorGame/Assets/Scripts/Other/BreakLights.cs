using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakLights : MonoBehaviour
{
    [SerializeField] private GameObject[] StringLightsOn;
    [SerializeField] private GameObject[] StringLightsOff;

    public void BreakStringLights()
    {
        foreach (GameObject sl in StringLightsOn)
        {
            sl.SetActive(false);
        }

        foreach (GameObject sl in StringLightsOff)
        {
            sl.SetActive(true);
        }
    }

    public void FixStringLights()
    {
        foreach (GameObject sl in StringLightsOn)
        {
            sl.SetActive(true);
        }

        foreach (GameObject sl in StringLightsOff)
        {
            sl.SetActive(false);
        }
    }
}
