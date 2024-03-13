using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private LevelManager levelManager;
    private void Awake()
    {
        levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();

        QuotaData quotaData = new QuotaData();
        ScoreData scoreData = new ScoreData();
        
    }

    public void playGame()
    {
      //  StartCoroutine(levelManager.loadLevel(1));
        
        // SceneManager.LoadScene("BuildSubmissionV1");


    }

    public void quitGame()
    {
        Application.Quit();
    }

}
