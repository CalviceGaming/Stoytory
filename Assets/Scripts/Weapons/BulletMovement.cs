using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private GameObject trail;
    
    private Rigidbody rb;
    float despawnTimer = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;  // Aim at the hit object
        }
        else
        {
            targetPoint = ray.origin + ray.direction * 1000;
        }
        Vector3 direction = targetPoint - transform.position;

        rb.AddForce(direction.normalized * 30, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.tag != "Weapon")
        {
            Destroy(gameObject);
            if (other.tag == "Enemy")
            {
                other.GetComponent<EnemyHealthComponent>().DealDamage(damage);
            }
        }
    }

    void Update()
    {
        BulletDespawn();
    }

    private void BulletDespawn()
    {
        if (despawnTimer >= 10f)
        {
            Destroy(gameObject);
        }
        despawnTimer += Time.deltaTime;
    }
}