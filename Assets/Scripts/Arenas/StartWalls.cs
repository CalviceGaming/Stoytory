using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWalls : MonoBehaviour
{
    private List<GameObject> walls = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform) 
        {
            if (child.CompareTag("Wall")) 
            {
                walls.Add(child.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SpawnWalls()
    {
        foreach (GameObject wall in walls)
        {
            wall.SetActive(true);
        }
    }

    public void CloseArena()
    {
        foreach (GameObject wall in walls)
        {
            wall.SetActive(false);
        }
    }
}
