using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SendPlayerToEnding : MonoBehaviour
{
    [SerializeField] private GameObject[] thingsToDestroy;
    private bool change = false;
    private int frames = 0;
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
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject thing in thingsToDestroy)
            {
                Destroy(thing);
            }
            Cursor.lockState = CursorLockMode.None;
            change = true;
        }
    }

    void Update()
    {
        if (change)
        {
            if (frames > 1)
            {
                SceneManager.LoadScene("Ending");
                Destroy(gameObject);
            }
            frames++;
        }
    }
}
