using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEnemies : MonoBehaviour
{
    private List<GameObject> enemies = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform) 
        {
            if (child.CompareTag("Enemy")) 
            {
                enemies.Add(child.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(true);
        }
    }
}
