using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveWalls : MonoBehaviour
{
    [SerializeField] private GameObject enemies;

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
        enemies.GetComponent<EndArena>().allEnemiesDied.AddListener(DespawnWalls);
    }


    private void DespawnWalls()
    {
        foreach (GameObject wall in walls)
        {
            Destroy(wall);
        }
    }
}
