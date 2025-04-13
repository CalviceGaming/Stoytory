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
    // Start is called before the first frame update
    void Start()
    {
        enemyDied.AddListener(CheckEnemies);
        arenaSaver = GameObject.FindGameObjectWithTag("ArenaSaver");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckEnemies()
    {
        if (transform.childCount <= 1)
        {
            allEnemiesDied.Invoke();
        }
        arenaSaver.GetComponent<SaveArenas>().FinishArena(gameObject.transform.parent.GetComponent<ArenaId>().arenaId);
    }
}
