using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyPlayerAndHud : MonoBehaviour
{
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
