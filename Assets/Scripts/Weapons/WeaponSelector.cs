using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class WeaponSelector : MonoBehaviour
{
    [SerializeField] private InputActionReference pistolAction;
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject shotgun;
    [SerializeField] private GameObject rocketLauncher;
    [SerializeField] private GameObject pistolMagazine;
    [SerializeField] private InputActionReference shotgunAction;
    [SerializeField] private InputActionReference rocketAction;
    [SerializeField] private GameObject rocketMagazine;
    
    public bool shotgunUnlocked = false;
    public bool rocketUnlocked = false;
    
    public int swapIndex { get; private set; } = 1;

    private void OnEnable()
    {
        pistolAction.action.Enable();
        shotgunAction.action.Enable();
        rocketAction.action.Enable();
    }
    
    private void Start()
    {
        rocketLauncher.SetActive(false); 
        pistol.SetActive(false);
        pistolAction.action.started += OnPistolSwap;
        shotgunAction.action.started += OnShotgunSwap;
        rocketAction.action.started += OnRocketSwap;
    }

    private void OnPistolSwap(InputAction.CallbackContext callbackContext)
    {
        swapIndex = 1;
    }
    private void OnShotgunSwap(InputAction.CallbackContext callbackContext)
    {
        swapIndex = 2;
    }
    private void OnRocketSwap(InputAction.CallbackContext callbackContext)
    {
        swapIndex = 3;
    }

    private void Update()
    {
        switch (swapIndex)
        {
            case 1: pistolMagazine.SetActive(true);
                pistol.SetActive(true);
                shotgun.SetActive(false);
                rocketLauncher.SetActive(false);
                rocketMagazine.SetActive(false);
                break;
            case 2:
                if (!shotgunUnlocked)
                {
                    swapIndex = 1;
                    break;
                }
                pistol.SetActive(false); 
                shotgun.SetActive(true);
                pistolMagazine.SetActive(true);
                rocketLauncher.SetActive(false);
                rocketMagazine.SetActive(false);
                break;
            case 3:
                if (!rocketUnlocked)
                {
                    swapIndex = 1;
                    break;
                }
                pistol.SetActive(false); 
                shotgun.SetActive(false);
                pistolMagazine.SetActive(false);
                rocketMagazine.SetActive(true);
                rocketLauncher.SetActive(true);
                break;
        }
    }

    private void CheckIfUnlocked()
    {
        
    }
}
