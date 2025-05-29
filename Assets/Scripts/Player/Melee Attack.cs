using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{ 
    public GameObject slashPrefab; // assign your slash object in Inspector
    public Transform spawnPoint;  
    public float deactivateDelay = 0.5f;
    public Animator animator;
    public float meleeRadius = 2f; // reach
    [SerializeField] private float meleeForce = 500f; // How strong 
    [SerializeField] private float meleeCooldown = 3f; 
    [SerializeField] private float meleeDamage = 3f;
    private float cooldownTimer = 0f;
    
    private GameObject currentSlash;
    
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F) && cooldownTimer >= meleeCooldown)
        {
            //Debug.Log("Cooldown met. Performing melee.");
            PerformMelee();
            cooldownTimer = 0f; 
            SpawnSlash();
           // Debug.Log("F key was pressed!");
        }
    }
    
    
    private void PerformMelee()
    {
        bool hitSomething = false;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, meleeRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                Rigidbody enemyRb = hitCollider.GetComponent<Rigidbody>();
                if (enemyRb != null)
                {
                    Vector3 pushDirection = (hitCollider.transform.position - transform.position).normalized;
                    enemyRb.AddForce(pushDirection * meleeForce, ForceMode.Impulse);
                }
                
                EnemyHealthComponent enemyHealth = hitCollider.GetComponent<EnemyHealthComponent>();
                if (enemyHealth != null)
                {
                    enemyHealth.DealDamage(meleeDamage, transform.position);
                }
                hitSomething = true;
               // Debug.Log("Melee hit enemy: " + hitCollider.name);
            }
            
        }
        
        
        
    }

    void SpawnSlash()
    {
        //Debug.Log("SpawnSlash called");

        if (slashPrefab == null)
        {
            Debug.LogError("slashPrefab is not assigned in Inspector!");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("spawnPoint is not assigned in Inspector!");
            return;
        }

       

        if (currentSlash != null)
        {
            Destroy(currentSlash);
        }

        currentSlash = Instantiate(slashPrefab);
        currentSlash.transform.SetParent(spawnPoint);
        currentSlash.transform.localPosition = Vector3.zero;
        currentSlash.transform.localRotation = Quaternion.identity;
        currentSlash.SetActive(true);

        Animator anim = currentSlash.GetComponent<Animator>();
        if (anim != null)
        {
           // Debug.Log("Playing slash animation");
            anim.Play("Slash state", 0, 0f); // Make sure "Slash" is the animation state name
        }
        else
        {
           // Debug.LogWarning("Animator not found on slash prefab!");
        }

        StartCoroutine(DisableAfterDelay(deactivateDelay));
    }

    private System.Collections.IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (currentSlash != null)
        {
            currentSlash.SetActive(false);
        }
    }
    
    void OnDrawGizmos()
    {
        if (spawnPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(spawnPoint.position, spawnPoint.position + spawnPoint.forward * 2);
            Gizmos.DrawWireSphere(spawnPoint.position, 0.2f);
        }
    }
}
