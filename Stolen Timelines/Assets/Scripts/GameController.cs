using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public ScoreData scoreData;
    public QuotaData quotaData;
    public UIController uIController;
    public PlayerController pc = null;

    public bool isTimeUp = false;
    public bool isPlayerDead = false;
    

    private void Awake()
    {

       Screen.fullScreen = true;
       

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

    private void Start()
    {

    }
    public void increaseScore(int value)
    {

        scoreData.score += value;
        scoreData.itemsCollected++;
     //  Debug.Log("Score: " + scoreData.score);


    }

    private void Update()
    {

        if(pc== null && SceneManager.GetActiveScene().buildIndex ==2 )
        {
            findPlayer();
        }

        if (pc != null)
        {
           if( pc.health <= 0 && SceneManager.GetActiveScene().buildIndex == 2)
            {
                isPlayerDead = true;
            }

            else
            {
                isPlayerDead = false;
            }
        }
   
   

    }

    void findPlayer()
    {
      Debug.Log("Entered findPlayer");

        PlayerController foundPC;


      foundPC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (foundPC != null)
        {
            pc = foundPC;
         Debug.Log("Player not found");
            
        }
    }
}

        


