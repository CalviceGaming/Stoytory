using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthComponent health = other.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.respawnPoint = this.transform;
                Debug.Log("Checkpoint is now at" + transform.position);
            }
            else
            {
                Debug.LogWarning("HealthComponent not found on player.");
            }
        }
    }
}