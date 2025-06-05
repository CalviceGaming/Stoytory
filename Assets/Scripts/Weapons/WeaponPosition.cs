using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponPosition : MonoBehaviour
{
    [SerializeField] private GameObject weaponPos;
    private float recoil;
    private float reloadRecoil;
    public UnityEvent endReload;
    private float timer = 1f;
    
    //Audio
    [SerializeField] private AudioClip reloadAudio;
    [SerializeField] private AudioSource audioSource;
    private AudioManager audioManager;

    void Start()
    {
        if (GetComponent<PistolShooting>())
        {
            GetComponent<PistolShooting>().onShoot.AddListener(GunRecoil);
            GetComponent<PistolShooting>().onReload.AddListener(GunReload);
            timer = 1f;
        }
        if (GetComponent<ShotgunShooting>())
        {
            GetComponent<ShotgunShooting>().onShootShootgun.AddListener(GunRecoil);
            GetComponent<ShotgunShooting>().onReloadShootgun.AddListener(GunReload);
            timer = 2f;
        }

        if (GetComponent<RocketShooting>())
        {
            GetComponent<RocketShooting>().onShootRocket.AddListener(GunRecoil);
            GetComponent<RocketShooting>().onReloadRocket.AddListener(GunReload);
            timer = 5f;
        }
    }
    private void Update()
    {
        transform.position = weaponPos.transform.position;
        transform.rotation = weaponPos.transform.rotation * Quaternion.Euler(recoil + reloadRecoil, 0, 0);
    }

    private void GunRecoil()
    {
        LeanTween.value(gameObject, Rotation, 0, -55, 0.08f).setLoopPingPong(1);
    }

    private void GunReload()
    {
        if (!audioManager)
        {
            audioManager = FindObjectOfType<AudioManager>();
        }
        audioSource.volume = audioManager.volume;
        audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(reloadAudio);
        LeanTween.value(gameObject, RotationReload, 0, 360, timer).setOnComplete(InvokeEndReload);
    }

    private void Rotation(float rotation)
    {
        recoil = rotation;
    }
    private void RotationReload(float rotation)
    {
        reloadRecoil = rotation;
    }

    private void InvokeEndReload()
    {
        endReload.Invoke();
    }
}
