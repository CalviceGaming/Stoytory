using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject arenaTimer;
    
    private static GameObject instance;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); 
            return;
        }
        instance = gameObject;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
