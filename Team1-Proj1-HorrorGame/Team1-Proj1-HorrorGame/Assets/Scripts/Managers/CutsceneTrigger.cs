using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameSaveData _GameSaveData;
    [SerializeField] private GameObject cutscene;
    [SerializeField] private List<PlayableDirector> playableDirectors;

    [Header("Settings")]
    [SerializeField] private bool isOneTime;
    [SerializeField] private bool isOnAwake;
    [SerializeField] private bool isOnEnable;
    [SerializeField] private bool isOnTriggerEnter;

    [Header("Save Data")]
    public int cutsceneNum;
    public float endTime;

    void Awake()
    {
        if(isOnAwake)
        {
            cutscene.gameObject.GetComponent<PlayableDirector>().Play();
            _GameSaveData._currentCutscene = cutsceneNum;

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
            _GameSaveData._currentCutscene = cutsceneNum;

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
                _GameSaveData._currentCutscene = cutsceneNum;

                if(isOneTime)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void Skip()
    {
        PlayableDirector director = playableDirectors[_GameSaveData._currentCutscene];
        director.time = _GameSaveData._cutsceneEndTimes[_GameSaveData._currentCutscene];
    }

    public void EndCutscene()
    {
        PlayableDirector director = playableDirectors[_GameSaveData._currentCutscene];
        director.Stop();
    }
}
