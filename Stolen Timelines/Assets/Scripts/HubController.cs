using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubController : MonoBehaviour
{
    public ScoreData scoreData;
    public QuotaData quotaData;

    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI quotaText;
    [SerializeField]
    TextMeshProUGUI daysText;
    [SerializeField]
    TextMeshProUGUI titleText;
    [SerializeField]
    TextMeshProUGUI deathText;

    GameController gc;


    private float timer;

    private void Awake()
    {
   
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        

        scoreText.text = "Total Value Collected: ...";
        quotaText.text = "Quota Remaining: ...";
        daysText.text = "Days Remaining: ...";


        int currentDay = 4 - quotaData.daysLeft;
        titleText.text = "Daily Report - Day " + currentDay.ToString();
    }

    private void Start()
    {
        if (quotaData.gameStart == false)
        {
            quotaData.daysLeft -= 1;
            timer = 8f;
        }
        else if (quotaData.gameStart == true)
        {
            timer = 6f;
            Debug.Log("gameStart is true");
        }

        if (!gc.isPlayerDead && !gc.isTimeUp)
        {
            deathText.enabled = false;
        }
        else if (!gc.isPlayerDead && gc.isTimeUp)
        {
            deathText.text = "Extract before time runs out";
        }


    }

    void Update()
    {
        timer -= Time.deltaTime;


        if (quotaData.gameStart == false) //Hub entered from level
        {
            
            if (!gc.isPlayerDead) //Player didnt die in level and extracted
            {
                if (!gc.isTimeUp)
                {

                    if (timer >= 6f)
                    {
                        scoreText.text = "Total Value Collected: " + scoreData.score.ToString();

                    }
                    else if (timer >= 5f)
                    {
                        scoreText.text = "Total Value Collected: " + scoreData.score.ToString() + " ...selling";

                    }
                    else if (timer > 4f)
                    {
                        scoreText.text = "Total Value Collected: Sold";

                    }

                    if (timer < 4f)
                    {
                        sellScore();
                        quotaText.text = "Quota Remaining: " + quotaData.quotaRemain.ToString();
                    }

                    if (timer < 2f)
                    {
                        daysText.text = "Days Remaining: " + quotaData.daysLeft.ToString();

                    }


                    if (quotaData.quotaRemain <= 0)
                    {
                        quotaData.quotaLevel = quotaData.quotaLevel * 1.5f;
                        quotaData.quotaRemain = Mathf.RoundToInt(50 * quotaData.quotaLevel);

                        quotaData.daysLeft = 3;
                    }
                }

                else //Player ran out of time
                {
                    if (timer >= 6f)
                    {
                        scoreText.text = "You were Transported back with nothing";

                    }
                    else if (timer >= 5f)
                    {
                        scoreText.text = "Extract before the time runs out";

                    }
                    else if (timer > 4f)
                    {
                        scoreText.text = "Get good noob";

                    }

                    if (timer < 4f)
                    {
                        sellScore();
                        quotaText.text = "Quota Remaining: " + quotaData.quotaRemain.ToString();
                    }

                    if (timer < 2f)
                    {
                        daysText.text = "Days Remaining: " + quotaData.daysLeft.ToString();

                    }


                    if (quotaData.quotaRemain <= 0)
                    {
                        quotaData.quotaLevel = quotaData.quotaLevel * 1.5f;
                        quotaData.quotaRemain = Mathf.RoundToInt(200 * quotaData.quotaLevel);

                        quotaData.daysLeft = 3;
                    }
                }
            }

            else if (gc.isPlayerDead) //Player died in level
            {
                if (timer >= 6f)
                {
                    scoreText.text = "Total Value Collected: " + "You have nothing to sell";

                }
                else if (timer >= 5f)
                {
                    scoreText.text = "Total Value Collected: " + "...";

                }
                else if (timer > 4f)
                {
                    scoreText.text = "Total Value Collected: " + "Do better noob";

                }

                if (timer < 4f)
                {
                    sellScore();
                    quotaText.text = "Quota Remaining: " + quotaData.quotaRemain.ToString();
                }

                if (timer < 2f)
                {
                    daysText.text = "Days Remaining: " + quotaData.daysLeft.ToString();

                }


                if (quotaData.quotaRemain <= 0)
                {
                    quotaData.quotaLevel = quotaData.quotaLevel * 1.5f;
                    quotaData.quotaRemain = Mathf.RoundToInt(50 * quotaData.quotaLevel);

                    quotaData.daysLeft = 3;
                }
            }

        }

        else if (quotaData.gameStart == true) //Hub entered from main menu
        {
            quotaData.gameStart = false;

            if (timer < 4f)
            {
                sellScore();
                quotaText.text = "Quota Remaining: " + quotaData.quotaRemain.ToString();
            }

            if (timer < 2f)
            {
                daysText.text = "Days Remaining: " + quotaData.daysLeft.ToString();

            }


            if (quotaData.quotaRemain <= 0)
            {
                quotaData.quotaLevel = quotaData.quotaLevel * 1.5f;
                quotaData.quotaRemain = Mathf.RoundToInt(50 * quotaData.quotaLevel);

                quotaData.daysLeft = 3;
            }
        }



        

    }

    public void goAgain()
    {
        gc.isTimeUp = false;
        if (quotaData.daysLeft <= 0 && quotaData.quotaRemain > 0)
        {
            quotaData.quotaLevel = 1;
            quotaData.quotaRemain = 50;
            quotaData.daysLeft = 3;
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            SceneManager.LoadScene("BuildSubmissionV1");
        }
    }

    public void sellScore()
    {
        scoreData.itemsCollected = 0;
        quotaData.quotaRemain -= scoreData.score;
        scoreData.score = 0;
    }
}
