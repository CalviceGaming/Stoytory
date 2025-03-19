using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSelector : MonoBehaviour
{
    [SerializeField] private InputActionReference pistolAction;
    [SerializeField] private GameObject pistol;
    [SerializeField] private InputActionReference shotgunAction;
    [SerializeField] private InputActionReference rocketAction;
    private int swapIndex;

    private void OnEnable()
    {
        pistolAction.action.Enable();
        shotgunAction.action.Enable();
        rocketAction.action.Enable();
    }
    
    private void Start()
    {
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
            case 1: pistol.SetActive(true); break;
            case 2: pistol.SetActive(false); break;
            case 3: pistol.SetActive(false); break;
        }
    }
}
