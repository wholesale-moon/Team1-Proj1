using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerCutscene : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] private GameObject cutscene;
    [SerializeField] private GameObject player;

    [Header("Settings")]
    [SerializeField] private bool isOneTime;
    [SerializeField] private bool isOnAwake;
    [SerializeField] private bool isOnEnable;
    [SerializeField] private bool isOnTriggerEnter;

    private void OnTriggerEnter2D (Collider2D obj)
    {
        if (isOnTriggerEnter)
        {
                if(obj.gameObject.tag == "Player")
            {
                cutscene.GetComponent<PlayableDirector>().Play();
                if (isOneTime)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void EndCutscene()
    {
        cutscene.GetComponent<PlayableDirector>().Stop();
    }
}
