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
    private EndArena endArea;
    [SerializeField] private GameObject tileGenerator;
    public List<Vector3> tiles = new List<Vector3>();

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
        endArea = enemyParent.GetComponent<EndArena>();
        amountOf = Mathf.RoundToInt(Mathf.Sqrt(dificultyLevel) * 3);
        amountOfWaves = (amountOf/3);
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
            for (int i = 0; i < tileGenerator.transform.childCount; i++)
            {
            tiles.Add(tileGenerator.transform.GetChild(i).position);
            }
            Waves();
    }

    // Update is called once per frame
    void Update()
    {
        if (waveTimer > 10 && wave != amountOfWaves)
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
            Vector3 randomTile =tiles[Random.Range(0, tiles.Count)];
            Instantiate(greenSoldierPrefab, randomTile + Vector3.up*2, Quaternion.identity ,enemyParent.transform);
            amountOfSoldiers--;
        }
        for (int i = 0; i < ballsThisWave; i++)
        {
            Vector3 randomTile =tiles[Random.Range(0, tiles.Count)];
            Instantiate(bouncyBallPrefab, randomTile + Vector3.up*2, Quaternion.identity ,enemyParent.transform);
            amountOfBalls--;
        }
        for (int i = 0; i < dynosThisWave; i++)
        {
            Vector3 randomTile =tiles[Random.Range(0, tiles.Count)];
            Instantiate(dinossaurPrefab, randomTile + Vector3.up*4, Quaternion.identity ,enemyParent.transform);
            amountOfDinos--;
        }
        wave++;
        endArea.enemyAmount += greenSoldiersThisWave + ballsThisWave + dynosThisWave;
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
