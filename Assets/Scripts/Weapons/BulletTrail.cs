using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    [SerializeField] public GameObject bulletPrefab;

    private void Update()
    {
        transform.position = bulletPrefab.transform.position;
    }
}
