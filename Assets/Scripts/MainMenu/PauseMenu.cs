using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject settingsMenuUI;
    private bool isPaused = false;
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void BackToMenu()
    {
        pauseMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Trying to load MainMenu...");
        //Destroy(GameObject.FindGameObjectWithTag("Player"));
        SceneManager.LoadScene(0);
    }

    public void Setttings()
    {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);

    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
