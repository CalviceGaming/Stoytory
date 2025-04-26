using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangeTrigger : MonoBehaviour
{
    [SerializeField] private GameObject main;
    private DinossaurMelee dinossaurMelee;
    // Start is called before the first frame update
    void Start()
    {
        dinossaurMelee = main.GetComponent<DinossaurMelee>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            dinossaurMelee.attacking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            dinossaurMelee.attacking = false;
        }
    }
}
