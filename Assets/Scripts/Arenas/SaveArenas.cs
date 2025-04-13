using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveArenas : MonoBehaviour
{
    private List<int> arenaComplete = new List<int>();
    
    private static GameObject instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = gameObject;
        DontDestroyOnLoad(gameObject);
    }




    public bool CheckArenaComplete(int arena)
    {
        if (arenaComplete.Contains(arena))
        {
            return true;
        }

        return false;
    }

    public void FinishArena(int arena)
    {
        arenaComplete.Add(arena);
    }
}
