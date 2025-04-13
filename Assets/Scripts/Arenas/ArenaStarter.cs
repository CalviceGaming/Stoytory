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
    
    [SerializeField]private GameObject wallStarter;
    [SerializeField]private GameObject enemyStarter;
    [SerializeField]private GameObject tileStarter;
    

    private GameObject arenaSaver;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        arenaSaver = GameObject.FindGameObjectWithTag("ArenaSaver");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 middleGround = gameObject.transform.position;
        float distance = Vector3.Distance(middleGround, playerPos);
        if (playerOnArena && distance < 20 && !arenaStarted)
        {
            if (!arenaSaver.GetComponent<SaveArenas>().CheckArenaComplete(gameObject.transform.parent.GetComponent<ArenaId>().arenaId))
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
        wallStarter.GetComponent<StartWalls>().SpawnWalls();
        enemyStarter.GetComponent<StartEnemies>().SpawnEnemies();
        tileStarter.GetComponent<TileGenerator>().GenerateTiles();
    }
}
