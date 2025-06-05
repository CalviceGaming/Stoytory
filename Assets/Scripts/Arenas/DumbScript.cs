using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbScript : MonoBehaviour
{
    [SerializeField] private GameObject arena;
    private int arenaId;
    private GameObject enemies;
    private EndArena endArena;

    [SerializeField] private GameObject arenaSaver;
    private SaveArenas saveArenas;
    // Start is called before the first frame update
    void Start()
    {
        arenaSaver = GameObject.FindGameObjectWithTag("ArenaSaver");
        arenaId = arena.GetComponent<ArenaId>().arenaId;
        saveArenas = arenaSaver.GetComponent<SaveArenas>();
        foreach(Transform child in arena.transform)
        {
            if (child.GetComponent<EndArena>())
            {
                enemies = child.gameObject;
            }
        }
        endArena = enemies.GetComponent<EndArena>();
        endArena.endArena.AddListener(SetActive);
        if (arenaSaver.GetComponent<SaveArenas>().CheckArenaComplete(arenaId))
        {
            SetActive();
            GetComponent<TpObjectToMe>().Teleport();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }



    void SetActive()
    {
        gameObject.SetActive(true);
        GetComponent<TpObjectToMe>().Teleport();
    }
}
