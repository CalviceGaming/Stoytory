using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArenaStarter : MonoBehaviour
{
    private GameObject player;

    private bool playerOnArena;

    private bool arenaStarted;

    public bool playerInside = false;
    
    [SerializeField]private GameObject wallStarter;
    [SerializeField]private GameObject enemyStarter;
    [SerializeField]private GameObject tileStarter;
    private StartWalls startWalls;
    private StartEnemies startEnemies;
    private TileGenerator tileGenerator;
    

    private GameObject arenaSaver;
    private SaveArenas saveArenas;
    private ArenaId arenaId;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        arenaSaver = GameObject.FindGameObjectWithTag("ArenaSaver");
        startWalls = wallStarter.GetComponent<StartWalls>();
        startEnemies = enemyStarter.GetComponent<StartEnemies>();
        tileGenerator = tileStarter.GetComponent<TileGenerator>();
        saveArenas = arenaSaver.GetComponent<SaveArenas>();
        arenaId = gameObject.transform.parent.GetComponent<ArenaId>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 middleGround = gameObject.transform.position;
        if (playerInside && !arenaStarted)
        {
            if (!saveArenas.CheckArenaComplete(arenaId.arenaId))
            {
                arenaStarted = true;
                startExtras();   
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerOnArena = true;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerOnArena = false;
        }
    }

    private void startExtras()
    {
        startWalls.SpawnWalls();
        startEnemies.SpawnEnemies();
        tileGenerator.GenerateTiles();
    }
}
