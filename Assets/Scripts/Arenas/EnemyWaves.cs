using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWaves : MonoBehaviour
{
    [SerializeField] private int dificultyLevel = 1;
    [SerializeField] private GameObject greenSoldierPrefab;
    [SerializeField] private GameObject dinossaurPrefab;
    [SerializeField] private GameObject bouncyBallPrefab;
    [SerializeField] private GameObject enemyParent;
    public List<GameObject> tiles = new List<GameObject>();

    private int amountOf;
    private int amountOfSoldiers;
    private int amountOfDinos;
    private int amountOfBalls;

    private int amountOfWaves;
    private int wave = 0;
    private float waveTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        amountOf = Mathf.RoundToInt(Mathf.Sqrt(dificultyLevel*2) * 3);
        amountOfWaves = amountOf/3;
            if (dificultyLevel > 7)
            {
                for (int i = 0; i < Random.Range(1,2); i++)
                {
                    amountOfDinos++;
                    amountOf--;
                }
            }
            if (dificultyLevel > 3)
            {
                for (int i = 0; i < amountOf-amountOf/1.5; i++)
                {
                    amountOfBalls++;
                    amountOf--;
                }
            }
            for (int i = 0; i < amountOf; i++)
            {
                amountOfSoldiers++;
            }
            
            
            enemyParent.GetComponent<EndArena>().allEnemiesDied.AddListener(CheckWaves);
            Waves();
    }

    // Update is called once per frame
    void Update()
    {
        if (waveTimer > 30 && wave != amountOfWaves)
        {
            waveTimer = 0;
            Waves();
        }
        waveTimer += Time.deltaTime;
    }

    void Waves()
    {
        int greenSoldiersThisWave = AmountOfThisWave(amountOfSoldiers);
        int ballsThisWave = AmountOfThisWave(amountOfBalls);
        int dynosThisWave = AmountOfThisWave(amountOfDinos);
        for (int i = 0; i < greenSoldiersThisWave; i++)
        {
            GameObject randomTile =tiles[Random.Range(0, tiles.Count)];
            Instantiate(greenSoldierPrefab, randomTile.transform.position + Vector3.up*2, Quaternion.identity ,enemyParent.transform);
            amountOfSoldiers--;
        }
        for (int i = 0; i < ballsThisWave; i++)
        {
            GameObject randomTile =tiles[Random.Range(0, tiles.Count)];
            Instantiate(bouncyBallPrefab, randomTile.transform.position + Vector3.up*2, Quaternion.identity ,enemyParent.transform);
            amountOfBalls--;
        }
        for (int i = 0; i < dynosThisWave; i++)
        {
            GameObject randomTile =tiles[Random.Range(0, tiles.Count)];
            Instantiate(dinossaurPrefab, randomTile.transform.position + Vector3.up*4, Quaternion.identity ,enemyParent.transform);
            amountOfDinos--;
        }
        wave++;
    }

    void CheckWaves()
    {
        if (wave == amountOfWaves)
        {
            enemyParent.GetComponent<EndArena>().endArena.Invoke();
        }
        else
        {
            Waves();
        }
    }

    int AmountOfThisWave(int amountOfEnemy)
    {
        int amountThisWave = amountOfEnemy / amountOfWaves;
        if (amountThisWave * amountOfEnemy < amountOfEnemy)
        {
            amountThisWave++;
        }
        return amountThisWave;
    }
}
