using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PistolShooting : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletSpawn;
    [SerializeField] private GameObject bulletsParent;
    private float shootTimer = 3f;
    public UnityEvent onShoot;
    [SerializeField] private float maxMagazine = 10;
    private float currentMagazine;
    public UnityEvent onReload;
    private bool reloadingAnimation = false;
    [SerializeField] private Text magazineText;
    
    //Inputs
    [SerializeField] private InputActionReference shootingAction;
    [SerializeField] private bool shooting;
    [SerializeField] private InputActionReference reloadAction;
    [SerializeField] private bool reloading;


    private void OnEnable()
    {
        shootingAction.action.Enable();
        reloadAction.action.Enable();
    }

    private void Start()
    {
        shootingAction.action.started += OnShootingStarted;
        reloadAction.action.started += OnReloadingStarted;
        currentMagazine = maxMagazine;
        GetComponent<WeaponPosition>().endReload.AddListener(EndReload);
        magazineText.GetComponent<Text>().text = $"{currentMagazine}/{maxMagazine}";
    }
    
    private void OnShootingStarted(InputAction.CallbackContext callbackContext)
    {
        shooting = true;
    }
    private void OnReloadingStarted(InputAction.CallbackContext callbackContext)
    {
        reloading = true;
    }

    private void Update()
    {
        ShootingBullet();
        Reload();
    }

    private void ShootingBullet()
    {
        if (currentMagazine <= 0)
        {
            shooting = false;
            reloading = true;
            return;
        }
        if (shooting && shootTimer > 0.5f && !reloading)
        {
            shootTimer = 0f;
            currentMagazine--;
            magazineText.GetComponent<Text>().text = $"{currentMagazine}/{maxMagazine}";
            Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation, bulletsParent.transform);
            onShoot.Invoke();
        }
        shooting = false;
        shootTimer += Time.deltaTime;
    }

    private void Reload()
    {
        if (reloading && !shooting && !reloadingAnimation)
        {
            if (currentMagazine < maxMagazine)
            {
                onReload.Invoke();
                reloadingAnimation = true;
            }
            else
            {
                reloading = false;
            }
        }
    }

    private void EndReload()
    {
        currentMagazine = maxMagazine;
        magazineText.GetComponent<Text>().text = $"{currentMagazine}/{maxMagazine}";
        reloading = false;
        reloadingAnimation = false;
    }
}
