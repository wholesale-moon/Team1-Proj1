using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject cutscene;
    [SerializeField] private List<PlayableDirector> playableDirectors;

    [Header("Settings")]
    [SerializeField] private bool isOneTime;
    [SerializeField] private bool isOnAwake;
    [SerializeField] private bool isOnEnable;
    [SerializeField] private bool isOnTriggerEnter;

    [SerializeField] private float endTime;

    void Awake()
    {
        if(isOnAwake)
        {
            cutscene.gameObject.GetComponent<PlayableDirector>().Play();
            if(isOneTime)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnEnable()
    {
        if(isOnEnable)
        {
            cutscene.gameObject.GetComponent<PlayableDirector>().Play();
            if(isOneTime)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.gameObject.tag == "Player")
        {
            if(isOnTriggerEnter)
            {
                cutscene.gameObject.GetComponent<PlayableDirector>().Play();
                if(isOneTime)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void Skip()
    {
        //Skip to end of cutscene?
    }

    public void EndCutscene()
    {
        foreach (PlayableDirector director in playableDirectors)
        {
            director.Stop();
        }
    }
}
