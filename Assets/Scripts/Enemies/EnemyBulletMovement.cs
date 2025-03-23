using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletMovement : MonoBehaviour
{
    [SerializeField] private int speedForce = 20;
    [SerializeField] private float damage;
    [SerializeField] private GameObject breakEffect;
    [SerializeField] private GameObject bulletObjects;
    private GameObject player;
    
    private Rigidbody rb;
    float despawnTimer = 0f;
    private bool toDestroy = false;
    private float destroyTimer = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");


        Vector3 direction = player.transform.position - transform.position;

        rb.AddForce(direction.normalized * speedForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag != "Enemy" && other.gameObject.tag != "Weapon")
        {
            rb.velocity = Vector3.zero;
            bulletObjects.SetActive(false);
            Instantiate(breakEffect).GetComponent<DestroyBulletParticle>().bullet = gameObject;
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
