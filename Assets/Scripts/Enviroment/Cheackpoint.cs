using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public ParticleSystem ParticleSystem;
    public int checkpointNumber = 1;
    
    private bool hasPlayed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (hasPlayed) return;
        
        if (other.CompareTag("Player"))
        {
            HealthComponent healthComponent = other.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                //  respawn point Update
                healthComponent.SetRespawnPoint(checkpointNumber);
                ParticleSystem.Play();
                hasPlayed = true;
                
                

            }
        }
    }
}