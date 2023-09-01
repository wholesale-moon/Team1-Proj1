using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 4;
    public int currentHealth;

    [Space(10)]
    [SerializeField] private GameObject medBottle;
    
    [Space(10)]
    [SerializeField] private GameObject healthbar;
    [SerializeField] private GameObject[] hearts;

    private bool setHealthPrologue = false;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            currentHealth = 3;
        } else {
            currentHealth = maxHealth;
            foreach (GameObject heart in hearts)
            {
                heart.GetComponent<Animator>().SetBool("isFull", true);
            }
        }
    }

    private void Update()
    {
        if(!setHealthPrologue && healthbar.activeSelf && SceneManager.GetActiveScene().buildIndex == 1)
        {
            PrologueHealth();
            setHealthPrologue = true;
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

    public void PrologueHealth()
    {
        hearts[0].GetComponent<Animator>().SetBool("isFull", true);
        hearts[1].GetComponent<Animator>().SetBool("isFull", true);
        hearts[2].GetComponent<Animator>().SetBool("isFull", true);
        hearts[3].GetComponent<Animator>().SetBool("isFull", false);
    }

    public IEnumerator MedicineSpawn()
    {
        yield return new WaitForSeconds(25);

        medBottle.SetActive(true);
        yield return null;
    }
}
