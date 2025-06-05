using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTextOnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject textObject;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textObject.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textObject.SetActive(false);
        }
    }
}
