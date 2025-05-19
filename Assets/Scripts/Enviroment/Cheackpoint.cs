using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointNumber = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthComponent healthComponent = other.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                //  respawn point Update
                healthComponent.SetRespawnPoint(checkpointNumber);
            }
        }
    }
}