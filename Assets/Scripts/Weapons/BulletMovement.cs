using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private int speedForce = 100;
    [SerializeField] public float damage{ private get; set; }
    [SerializeField] private GameObject breakEffect;
    [SerializeField] private GameObject bulletObjects;
    
    private Rigidbody rb;
    float despawnTimer = 0f;
    private bool toDestroy = false;
    private float destroyTimer = 0f;
    
    public Vector3 directionSet { private get; set; } = Vector3.zero;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
            rb.velocity = Vector3.zero;
            bulletObjects.SetActive(false);
            GetComponent<CapsuleCollider>().enabled = false;

            if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<EnemyHealthComponent>().DealDamage(damage, transform.position);
            }
            else
            {
                Instantiate(breakEffect).GetComponent<DestroyBulletParticle>().bullet = gameObject;
            }

            toDestroy = true;
        }
    }
    
    

    void Update()
    {
        BulletDespawn();
        toDestroyBullet();
    }

    private void BulletDespawn()
    {
        if (!toDestroy)
        {
            if (despawnTimer >= 10f)
            {
                Destroy(gameObject);
            }
            despawnTimer += Time.deltaTime;   
        }
    }

    private void toDestroyBullet()
    {
        if (toDestroy)
        {
            if (destroyTimer >= 0.6f)
            {
                Destroy(gameObject);
            }
            destroyTimer += Time.deltaTime;
        }
    }
}