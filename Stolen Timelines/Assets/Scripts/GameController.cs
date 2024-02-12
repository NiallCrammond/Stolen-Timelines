using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public ScoreData scoreData;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
  
        scoreData.score = 0;
        scoreData.itemsCollected = 0;
       

    }
    public void increaseScore()
    {
        scoreData.score += 10;
        scoreData.itemsCollected++;
    }

}