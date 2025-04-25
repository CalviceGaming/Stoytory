using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaHitBox : MonoBehaviour
{
    [SerializeField] private GameObject arena;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            arena.GetComponent<ArenaStarter>().playerInside = true;
            Destroy(gameObject);
        }
    }
}
