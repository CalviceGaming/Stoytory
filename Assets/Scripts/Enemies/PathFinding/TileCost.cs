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
        transform.GetChild(0).GetComponent<TileWallChecker>().started.AddListener(CheckIfWall);
    }

    private void CheckIfWall()
    {
        RaycastHit hit;
        if (transform.GetChild(0).GetComponent<TileWallChecker>().wall)
        {
            tileCost = 1000;
            wall = true;
        }
    }

    private void CheckNeighbours()
    {
        Vector3 pos = transform.position;

        // Possible movement directions (now including up/down variations)
        Vector3[] directions = new Vector3[]
        {
            new Vector3(10, 0, 0),  // Right
            new Vector3(-10, 0, 0), // Left
            new Vector3(0, 0, 10),  // Forward
            new Vector3(0, 0, -10), // Backward
            new Vector3(10, 10, 0),  // Up slope right
            new Vector3(-10, 10, 0), // Up slope left
            new Vector3(0, 10, 10),  // Up slope forward
            new Vector3(0, 10, -10), // Up slope backward
            new Vector3(10, -10, 0),  // Down slope right
            new Vector3(-10, -10, 0), // Down slope left
            new Vector3(0, -10, 10),  // Down slope forward
            new Vector3(0, -10, -10)  // Down slope backward
        };

        float yTolerance = 2f; // Maximum allowed height difference

        foreach (Vector3 dir in directions)
        {
            RaycastHit hit;
            if (Physics.Raycast(pos, dir, out hit, 5))
            {
                if (hit.collider.tag == "Tile")
                {
                    if (hit.collider.gameObject.GetComponent<TileCost>().wall && !wall)
                    {
                        tileCost = 1000;
                    }   
                }
            }
        }
    }

    public float ReturnCost()
    {
        CheckNeighbours();
        return tileCost;
    }
}
