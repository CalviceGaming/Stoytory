using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PistolShooting : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletSpawn;
    private float shootTimer = 3f;
    
    //Inputs
    [SerializeField] private InputActionReference shootingAction;
    [SerializeField] private bool shooting;


    private void OnEnable()
    {
        shootingAction.action.Enable();
    }

    private void Start()
    {
        shootingAction.action.started += OnShootingStarted;
    }
    
    private void OnShootingStarted(InputAction.CallbackContext callbackContext)
    {
        shooting = true;
    }

    private void Update()
    {
        ShootingBullet();
    }

    private void ShootingBullet()
    {
        if (shooting && shootTimer > 0.5f)
        {
            shooting = false;
            shootTimer = 0f;
            Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        }
        shootTimer += Time.deltaTime;
    }
}
