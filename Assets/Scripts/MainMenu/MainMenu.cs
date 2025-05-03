using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("FirstScene");
        Time.timeScale = 1f;
    }
    
        
    public void QuitGame()
    { 
        Application.Quit();
    }

}
