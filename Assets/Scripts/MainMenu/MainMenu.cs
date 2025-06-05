using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject player;
    private GameObject weapon;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        if (player)
        {
            player.SetActive(false);
            weapon = GameObject.FindGameObjectWithTag("Weapon");
            weapon.SetActive(false);
        }
    }
    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("Map");
        Time.timeScale = 1f;
        if (player)
        {
            player.SetActive(true);
            weapon.SetActive(false);
        }
    }
    
        
    public void QuitGame()
    { 
        Application.Quit();
    }

}
