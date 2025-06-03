using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKey : MonoBehaviour
{
    [SerializeField] private AudioClip keyNote;
    [SerializeField] private float pitch = 1;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioManager.PlaySound(keyNote, pitch);
        }
    }
}
