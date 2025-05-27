using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    private static GameObject instance;
    void Awake()
    {
        if (transform.parent == null)
            DontDestroyOnLoad(gameObject);
    }
}
