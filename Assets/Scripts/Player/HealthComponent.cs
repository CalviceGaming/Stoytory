using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int maxHealth = 15;
    private int currentHealth;
    private float timeSinceLastDamage;
    private float timeForHeal;

    [SerializeField] private GameObject healthText;

    [SerializeField] private GameObject instance;

    private Rigidbody rb;

    private List<GameObject> checkpoints = new List<GameObject>();
    
    private bool invincible = false;
    public Transform respawnPoint;
    
    [SerializeField] private GameObject healthContainer;
    [SerializeField] private GameObject healthUI;

    void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("Player").Length > 1)
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        currentHealth = maxHealth;
        UpdateUIHealth();
        rb = GetComponent<Rigidbody>();
        foreach (GameObject checkpoint in GameObject.FindGameObjectsWithTag("Checkpoints"))
        {
            checkpoints.Add(checkpoint);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();

        InvinsibleCheat();
    }
    
    public void SetRespawnPoint(Transform newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
        Debug.Log("Respawn point updated to: " + respawnPoint.position);
    }

    void CheckHealth()
    {
        if (timeSinceLastDamage >= 10)
        {
            if (timeForHeal >= 3 && currentHealth < maxHealth)
            {
                timeForHeal = 0;
                currentHealth++;
                UpdateUIHealth();
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
            UpdateUIHealth();
        }
    }
    
    void Respawn()
    {
        currentHealth = maxHealth;
        UpdateUIHealth();

        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position; 
            rb.velocity = Vector3.zero;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Reload the scene
        }
        else
        {
            Debug.LogWarning("Respawn point is null. Using default position.");
            transform.position = new Vector3(0, 0.3f, 0);  // Default respawn position 
            rb.velocity = Vector3.zero;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Reload the scene
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
            UpdateUIHealth();
        }
    }

    void UpdateUIHealth()
    {
        float percentage = (float) currentHealth / maxHealth;
        healthUI.GetComponent<Image>().fillAmount = percentage;
        healthText.GetComponent<Text>().text = currentHealth.ToString();
    }
}
