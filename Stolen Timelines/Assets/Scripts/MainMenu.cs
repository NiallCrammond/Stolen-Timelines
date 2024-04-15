using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{ 

    [SerializeField]
    private QuotaData quotaData;
    [SerializeField]
    private ScoreData scoreData;

    private LevelManager levelManager;
    private void Awake()
    {
        levelManager = GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>();              
        
    }

    public void playGame()
    {
        quotaData.quotaRemain = 50;
        quotaData.quotaLevel = 1;
        quotaData.daysLeft = 3;
        scoreData.score = 0;
        scoreData.itemsCollected = 0;
    }

    public void quitGame()
    {
        Application.Quit();
    }

}
