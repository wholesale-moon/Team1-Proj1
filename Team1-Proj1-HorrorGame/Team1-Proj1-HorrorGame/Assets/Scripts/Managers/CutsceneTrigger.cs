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
    [SerializeField] private bool isOnCollision;

    [Header("Save Data")]
    public int cutsceneNum;
    public float endTime;

    void Awake()
    {
        #if UNITY_EDITOR
        _GameSaveData._currentCutscene = 0;
        _GameSaveData.isPlayingCutscene = true;
        #endif
        
        if(isOnAwake)
        {
            cutscene.gameObject.GetComponent<PlayableDirector>().Play();
            _GameSaveData._currentCutscene = cutsceneNum;
            _GameSaveData.isPlayingCutscene = true;

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
            _GameSaveData._currentCutscene = cutsceneNum;
            _GameSaveData.isPlayingCutscene = true;

            if(isOneTime)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D obj)
    {
        if(obj.gameObject.tag == "Player")
        {
            if(isOnTriggerEnter)
            {
                cutscene.gameObject.GetComponent<PlayableDirector>().Play();
                _GameSaveData._currentCutscene = cutsceneNum;
                _GameSaveData.isPlayingCutscene = true;

                if(isOneTime)
                {
                    Destroy(gameObject);
                }
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
                _GameSaveData.isPlayingCutscene = true;

                if(isOneTime)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    // public void StartCutscene()
    // {
    //     _GameSaveData._currentCutscene = 2;
    //     PlayableDirector director = playableDirectors[_GameSaveData._currentCutscene];
    //     cutscene.gameObject.GetComponent<PlayableDirector>().Play();
    // }

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
