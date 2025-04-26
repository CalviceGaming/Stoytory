using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("FirstScene");
        Time.timeScale = 1f;
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<HealthComponent>().DealDamage(1000);
        }
    }
    
        
    public void QuitGame()
    { 
        Application.Quit();
    }

}
