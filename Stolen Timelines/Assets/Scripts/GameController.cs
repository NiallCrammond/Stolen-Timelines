using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public ScoreData scoreData;
    public QuotaData quotaData;
    public UIController uIController;

    

    private void Awake()
    {

        if (instance == null)
        {
            scoreData.score = 0;
            scoreData.itemsCollected = 0;
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
  
       

    }
    public void increaseScore()
    {

        scoreData.score += 10;
        scoreData.itemsCollected++;
       Debug.Log("Score: " + scoreData.score);


    }
}
