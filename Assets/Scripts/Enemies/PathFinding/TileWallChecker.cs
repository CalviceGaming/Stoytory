using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileWallChecker : MonoBehaviour
{
    public bool wall;

    public UnityEvent started;
    // Start is called before the first frame update
    void Start()
    {
        started.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Wall")
        {
            wall = true;
        }
    }
}
