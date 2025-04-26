using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{ 
    public float meleeRadius = 2f; // reach
    [SerializeField] private float meleeForce = 500f; // How strong 
    [SerializeField] private float meleeCooldown = 3f; 
    [SerializeField] private float meleeDamage = 3f;
    private float cooldownTimer = 0f;
    
    
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F) && cooldownTimer >= meleeCooldown)
        {
            PerformMelee();
            cooldownTimer = 0f; 
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
                Debug.Log("Melee hit enemy: " + hitCollider.name);
            }
            
        }
        if (!hitSomething)
        {
            Debug.Log("Your trash kid"); 
        }
    }
}
