using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBulletParticle : MonoBehaviour
{
    public GameObject bullet;
    private ParticleSystem particle;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        Vector3 dir = (player.transform.position - bullet.transform.position).normalized;
        
        transform.position = bullet.transform.position + (dir * 0.1f);
        
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!particle.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
