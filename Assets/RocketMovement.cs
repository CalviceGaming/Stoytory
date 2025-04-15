using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    [SerializeField] private int speedForce = 30;             // Slower than bullets, typical rocket speed
    [SerializeField] private float damage;                    // Damage dealt by the rocket
    [SerializeField] private GameObject breakEffect;          // Explosion effect when rocket hits
    [SerializeField] private GameObject rocketObjects;        // The rocket object itself
    
    private Rigidbody rb;
    private float despawnTimer = 0f;                           // Time before rocket despawns if no collision
    private bool toDestroy = false;                            // Flag to track if rocket should be destroyed
    private float destroyTimer = 0f;

    public Vector3 directionSet { private get; set; } = Vector3.zero; // Direction of rocket movement

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Handle direction based on where the rocket is supposed to go
        if (directionSet == Vector3.zero)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point; 
            }
            else
            {
                targetPoint = ray.origin + ray.direction * 200;
            }

            Vector3 direction = targetPoint - transform.position;
            rb.AddForce(direction.normalized * speedForce, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(directionSet.normalized * speedForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "Weapon" && other.gameObject.tag != "Bullets")
        {
            rb.velocity = Vector3.zero;             // Stop rocket immediately upon collision
            rocketObjects.SetActive(false);         // Disable the rocket object
            GetComponent<CapsuleCollider>().enabled = false;  // Disable the collider to avoid further collisions

            // Handle the rocket's effects based on what it collided with
            if (other.gameObject.tag == "Enemy")
            {
                // Deal damage to enemy
                other.gameObject.GetComponent<EnemyHealthComponent>().DealDamage(damage, transform.position);
            }
            
            // Instantiate the break/explosion effect
            Instantiate(breakEffect, transform.position, Quaternion.identity);

            toDestroy = true;  // Set the flag to destroy the rocket after some time
        }
    }

    void Update()
    {
        RocketDespawn();
        toDestroyRocket();
    }

    private void RocketDespawn()
    {
        if (!toDestroy)
        {
            // Destroy rocket after a certain amount of time if it didn't collide with anything
            if (despawnTimer >= 10f)
            {
                Destroy(gameObject);
            }
            despawnTimer += Time.deltaTime;
        }
    }

    private void toDestroyRocket()
    {
        if (toDestroy)
        {
            // Destroy rocket after explosion effect has played
            if (destroyTimer >= 0.6f)
            {
                Destroy(gameObject);
            }
            destroyTimer += Time.deltaTime;
        }       
    }
}
