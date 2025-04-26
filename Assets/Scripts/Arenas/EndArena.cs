using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndArena : MonoBehaviour
{
    public UnityEvent enemyDied;

    public UnityEvent endArena;
    
    public UnityEvent allEnemiesDied;

    private GameObject arenaSaver;

    public int enemyAmount;
    // Start is called before the first frame update
    void Start()
    {
        enemyDied.AddListener(CheckEnemies);
        arenaSaver = GameObject.FindGameObjectWithTag("ArenaSaver");
        endArena.AddListener(SaveArena);
    } 

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckEnemies()
    {
        enemyAmount--;
        if (enemyAmount < 1)
        {
            allEnemiesDied.Invoke();
        }
    }

    private void SaveArena()
    {
        arenaSaver.GetComponent<SaveArenas>().FinishArena(gameObject.transform.parent.GetComponent<ArenaId>().arenaId);
    }
}
