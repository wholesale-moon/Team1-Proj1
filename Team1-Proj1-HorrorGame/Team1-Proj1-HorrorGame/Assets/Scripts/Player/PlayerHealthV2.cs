using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 4;
    public int currentHealth;

    [SerializeField] private GameObject medBottle;
    [SerializeField] private GameObject[] hearts;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            currentHealth = 3;
            hearts[0].GetComponent<Animator>().SetBool("isFull", true);
            hearts[1].GetComponent<Animator>().SetBool("isFull", true);
            hearts[2].GetComponent<Animator>().SetBool("isFull", true);
            hearts[3].GetComponent<Animator>().SetBool("isFull", false);
        } else {
            currentHealth = maxHealth;
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
            hearts[currentHealth].GetComponent<Animator>().SetBool("isFull", false);
        }
        else if (currentHealth <= 1) //if the player is on the last heart
        {
            currentHealth = 0;
            hearts[currentHealth].GetComponent<Animator>().SetBool("isFull", false);
        } 
        else if (currentHealth <= 0) 
        {
            currentHealth = 0; // Player is already dead
        }
    }

    public void Heal()
    {
        hearts[currentHealth].GetComponent<Animator>().SetBool("isFull", true);
        currentHealth += 1;
    }

    public IEnumerator MedicineSpawn()
    {
        yield return new WaitForSeconds(25);

        medBottle.SetActive(true);
        yield return null;
    }
}
