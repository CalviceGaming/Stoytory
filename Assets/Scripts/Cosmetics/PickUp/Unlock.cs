using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlock : MonoBehaviour
{
    [SerializeField] private int unlockIndex; //1 shotgun 2 rocket
    private GameObject player;
    private WeaponSelector weaponSelector;
    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        weaponSelector = player.GetComponent<WeaponSelector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (unlockIndex == 1)
            {
                weaponSelector.shotgunUnlocked = true;
                Destroy(gameObject);
            }
            else if (unlockIndex == 2)
            {
                weaponSelector.rocketUnlocked = true;
                Destroy(gameObject);
            }
        }
    }
}
