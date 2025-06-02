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

    [SerializeField] private GameObject healthTextGameObject;
    private Text healthText;

    [SerializeField] private Rigidbody rb;

    private List<GameObject> checkpoints = new List<GameObject>();
    
    private bool invincible = false;
    private int respawnNumber;
    
    [SerializeField] private GameObject healthContainer;
    [SerializeField] private GameObject healthUI;
    private Image healthUIImage;
    
    // private static GameObject instance;
    // void Awake()
    // {
    //     if (instance != null && instance != this)
    //     {
    //         Destroy(gameObject); 
    //         return;
    //     }
    //     instance = gameObject;
    // }
    
    // Start is called before the first frame update
    void Start()
    {
        BackFromMenu();
        healthText = healthTextGameObject.GetComponent<Text>();
        healthUIImage = healthUI.GetComponent<Image>();
        currentHealth = maxHealth;
        UpdateUIHealth();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();

        InvinsibleCheat();
    }
    
    public void SetRespawnPoint(int newRespawnPoint)
    {
        if (respawnNumber < newRespawnPoint)
        {
            respawnNumber = newRespawnPoint;
        }
        currentHealth = maxHealth;
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
        
        transform.position = checkpoints[respawnNumber].transform.position; 
        rb.velocity = Vector3.zero;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Reload the scene
        
        // else
        // {
        //     Debug.LogWarning("Respawn point is null. Using default position.");
        //     transform.position = new Vector3(0, 0.3f, 0);  // Default respawn position 
        //     rb.velocity = Vector3.zero;
        //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Reload the scene
        // }

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
        healthUIImage.fillAmount = percentage;
        healthText.text = currentHealth.ToString();
    }

    public void BackFromMenu()
    {
        checkpoints.Clear();
        GameObject[] allCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoints");
        for(int i = 0; i < allCheckpoints.Length; i++)
        {
            foreach (GameObject checkpoint in allCheckpoints)
            {
                if (checkpoint.GetComponent<Checkpoint>().checkpointNumber == i)
                {
                    checkpoints.Add(checkpoint);
                }
            }
        }
        rb.velocity = Vector3.zero;
        rb.position = checkpoints[respawnNumber].transform.position;
        //transform.position = checkpoints[respawnNumber].transform.position;
    }
}
