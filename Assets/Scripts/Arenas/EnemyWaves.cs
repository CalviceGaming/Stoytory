using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWaves : MonoBehaviour
{
    public int dificultyLevel {private get;  set;}
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyParent;
    public List<GameObject> tiles = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
            int amountOf = Mathf.RoundToInt(Mathf.Sqrt(dificultyLevel) + 3);
            for (int i = 0; i < amountOf; i++)
            {
                GameObject randomTile =tiles[Random.Range(0, tiles.Count)];
                Instantiate(enemyPrefab, randomTile.transform.position + Vector3.up, Quaternion.identity ,enemyParent.transform);
            }
            dificultyLevel++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
