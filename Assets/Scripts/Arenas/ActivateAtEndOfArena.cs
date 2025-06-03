using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActivateAtEndOfArena : MonoBehaviour
{
    [SerializeField] private GameObject arena;
    private GameObject enemies;
    private EndArena endArena;

    [SerializeField] private GameObject arenaSaver;
    // Start is called before the first frame update
    void Start()
    {
        arenaSaver = GameObject.FindGameObjectWithTag("ArenaSaver");
        foreach(Transform child in arena.transform)
        {
            if (child.GetComponent<EndArena>())
            {
                enemies = child.gameObject;
            }
        }
        endArena = enemies.GetComponent<EndArena>();
        endArena.endArena.AddListener(SetActive);
        if (arenaSaver.GetComponent<SaveArenas>().CheckArenaComplete(arena.GetComponent<ArenaId>().arenaId))
        {
            SetActive();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void SetActive()
    {
        gameObject.SetActive(true);
    }
}
