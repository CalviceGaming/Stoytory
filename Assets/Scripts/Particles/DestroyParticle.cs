using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    private ParticleSystem particle;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }
    
    void Update()
    {
        if (!particle.IsAlive())
        {
            Destroy(gameObject);
            if (transform.parent)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
