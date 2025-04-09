using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int maxHealth = 15;
    private int currentHealth;
    private float timeSinceLastDamage;
    private float timeForHeal;

    [SerializeField] private GameObject healthText;
    
    private bool invincible = false;
    public Transform respawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthText.GetComponent<Text>().text = currentHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();

        InvinsibleCheat();
    }

    void CheckHealth()
    {
        if (timeSinceLastDamage >= 10)
        {
            if (timeForHeal >= 3 && currentHealth < maxHealth)
            {
                timeForHeal = 0;
                currentHealth++;
                healthText.GetComponent<Text>().text = currentHealth.ToString();
            }

            if (currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
            }
            else
            {
                timeForHeal += Time.deltaTime;
            }
        }
        timeSinceLastDamage += Time.deltaTime;
    }

    public void DealDamage(int damage)
    {
        if (invincible == false)
        {
            currentHealth -= damage;
            timeSinceLastDamage = 0;
            if (currentHealth <= 0)
            {
                Respawn();
            }
            healthText.GetComponent<Text>().text = currentHealth.ToString();   
        }
    }
    
    void Respawn()
    {
        currentHealth = maxHealth;
        healthText.GetComponent<Text>().text = currentHealth.ToString();
        
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        
        if (respawnPoint != null)
        {
            Debug.Log("Respawning at: " + respawnPoint.position);
            transform.position = respawnPoint.position;
        }
        else
        {
            Debug.LogWarning("Respawn point is null. Using default position.");
        }

        // Reset timers
        timeSinceLastDamage = 0;
        timeForHeal = 0;
    }

    void InvinsibleCheat()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            invincible = !invincible;
            currentHealth = maxHealth;
            healthText.GetComponent<Text>().text = currentHealth.ToString();
        }
    }
}
