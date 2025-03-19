using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPosition : MonoBehaviour
{
    [SerializeField] private GameObject weaponPos;

    private void Update()
    {
        transform.position = weaponPos.transform.position;
        transform.rotation = weaponPos.transform.rotation;
    }
}
