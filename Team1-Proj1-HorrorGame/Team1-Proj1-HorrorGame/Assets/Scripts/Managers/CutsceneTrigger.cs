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
    [SerializeField] private bool isTransition;

    void Awake()
    {
        if(isOnAwake)
        {
            cutscene.gameObject.GetComponent<PlayableDirector>().Play();
            if (!isTransition)
            {
                _GameSaveData._currentCutscene = cutsceneNum;
                _GameSaveData.isPlayingCutscene = true;
            }

            if(isOneTime)
            {
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(_GameSaveData.isPlayingCutscene)
            {
                Skip();
            }
        }
    }

    void OnEnable()
    {
        if(isOnEnable)
        {
            cutscene.gameObject.GetComponent<PlayableDirector>().Play();
            if (!isTransition)
            {
                _GameSaveData._currentCutscene = cutsceneNum;
                _GameSaveData.isPlayingCutscene = true;
            }

            if(isOneTime)
            {
                Destroy(gameObject);
            }
        }
    }

    // private void OnCollisionEnter2D(Collision2D obj)
    // {
    //     if(obj.gameObject.tag == "Player")
    //     {
    //         if(isOnTriggerEnter)
    //         {
    //             cutscene.gameObject.GetComponent<PlayableDirector>().Play();
    //             if (!isTransition)
    //             {
    //                 _GameSaveData._currentCutscene = cutsceneNum;
    //                 _GameSaveData.isPlayingCutscene = true;
    //             }

    //             if(isOneTime)
    //             {
    //                 Destroy(gameObject);
    //             }
    //         }
    //     }
    // }

    void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.gameObject.tag == "Player")
        {
            if(isOnTriggerEnter)
            {
                cutscene.gameObject.GetComponent<PlayableDirector>().Play();
                if (!isTransition)
                {
                    _GameSaveData._currentCutscene = cutsceneNum;
                    _GameSaveData.isPlayingCutscene = true;
                }

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
        _GameSaveData.isPlayingCutscene = false;
    }

    public void EndCutscene()
    {
        PlayableDirector director = playableDirectors[_GameSaveData._currentCutscene];
        director.Stop();
        _GameSaveData.isPlayingCutscene = false;
    }
}
