using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    
    public void PlaySound(AudioClip clip, float pitch)
    {
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(clip);
    }
}
