using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCost : MonoBehaviour
{
    public float tileCost;
    public bool wall = false;

    private void Start()
    {
        transform.GetChild(0).GetComponent<TileWallChecker>().foundWall.AddListener(CheckIfWall);
    }

    private void CheckIfWall()
    {
        if (transform.GetChild(0).GetComponent<TileWallChecker>().wall)
        {
            tileCost = 1000;
            wall = true;
            Destroy(gameObject);
        }
    }
    

    public float ReturnCost()
    {
        //CheckNeighbours();
        return tileCost;
    }
}
