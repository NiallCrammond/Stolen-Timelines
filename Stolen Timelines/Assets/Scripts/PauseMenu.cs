using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public ScoreData scoreData;
    public GameController gameController;
    
    private GameObject pauseMenu;
    public GameObject settingsMenu;
    public bool isPaused = false;
    
    void Start()
    {
        pauseMenu = GameObject.FindWithTag("PauseMenu");
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }

    public void pauseGame()
    {
        AudioManager.instance.pauseAll();
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void resumeGame()
    {
        AudioManager.instance.resumeAll();
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void goToMainMenu()
    {
       // scoreData.score = 0;
        gameController.scoreData.score = 0;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
