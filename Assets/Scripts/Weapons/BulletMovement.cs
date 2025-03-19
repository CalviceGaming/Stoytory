using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float damage;
    
    private Rigidbody rb;
    float despawnTimer = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 30, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.tag != "Weapon")
        {
            Destroy(gameObject);
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
