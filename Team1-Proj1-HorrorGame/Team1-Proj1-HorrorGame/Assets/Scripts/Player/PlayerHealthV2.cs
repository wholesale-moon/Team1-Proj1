using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] GameObject _SceneManager;
    public int maxHealth = 4;
    public int currentHealth;

    [Space(10)]
    [SerializeField] private GameObject medBottle;
    
    [Space(10)]
    public GameObject healthbar;
    [SerializeField] private GameObject[] hearts;


    private void Start()
    {
        currentHealth = maxHealth;
        
        if (healthbar.activeSelf)
        {
            foreach (GameObject heart in hearts)
            {
                heart.GetComponent<Animator>().SetBool("isFull", true);
            }
        }
    }
    
    public void TakeDamage()
    {
        if (currentHealth > 1) //if the player has at least 2 hearts
        {
            currentHealth -= 1;
            _SceneManager.GetComponent<SoundManager>().PlayClipByName("PlayerHurt1");
            hearts[currentHealth].GetComponent<Animator>().SetBool("isFull", false);
        }
        else if (currentHealth <= 1) //if the player is on the last heart
        {
            // play player death
            currentHealth = 0;
            _SceneManager.GetComponent<SoundManager>().PlayClipByName("PlayerDeath");
            hearts[currentHealth].GetComponent<Animator>().SetBool("isFull", false);
        } 
        else if (currentHealth <= 0) 
        {
            currentHealth = 0; // Player is already dead
        }
    }

    public void Heal()
    {
        medBottle.SetActive(false);
        _SceneManager.GetComponent<SoundManager>().PlayClipByName("Healing");
        hearts[currentHealth].GetComponent<Animator>().SetBool("isFull", true);
        currentHealth += 1;
    }

    public IEnumerator MedicineSpawn()
    {
        yield return new WaitForSeconds(5);

        medBottle.SetActive(true);
        yield return null;
    }
}
