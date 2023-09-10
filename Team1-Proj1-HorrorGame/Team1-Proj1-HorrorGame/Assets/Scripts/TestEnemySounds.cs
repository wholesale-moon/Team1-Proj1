using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySounds : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClipArray;
    public GameObject soundTrigger;

    // Start is called before the first frame update
    void Start()
    {
        //audioSource.PlayOneShot(RandomClip());
        //StartCoroutine(EnemySound());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    AudioClip RandomClip()
    {
        return audioClipArray[Random.Range(0, audioClipArray.Length)];
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == soundTrigger)
        {
            audioSource.PlayOneShot(RandomClip());
        }
    }*/

    public IEnumerator EnemySound()
    {
        audioSource.PlayOneShot(RandomClip());
        yield return new WaitForSeconds(6);
    }
}
