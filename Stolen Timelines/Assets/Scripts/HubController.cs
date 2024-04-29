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

    private GameController gc;
    private LevelManager lM;

    private GameObject contButton;
    private GameObject loseScreen;
    private GameObject winScreen;
    private GameObject hubScreen;

    bool canPlayQuotaLoad1;
    bool canPlayQuotaLoad2;
    bool canPlayQuotaLoad3;
    bool canPlayQuotaLoad4;
    bool canPlayMoneyEarned;

    private float timer;

    private void Awake()
    {

        loseScreen = GameObject.FindWithTag("LoseScreen");
        winScreen = GameObject.FindWithTag("WinScreen");
        hubScreen = GameObject.FindWithTag("HubScreen");
        contButton = GameObject.FindWithTag("ContinueButton");
        contButton.SetActive(false);
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        lM = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();

        quotaData.daysLeft -= 1;
        timer = 8f;

        scoreText.text = "Total Value Collected: ...";
        quotaText.text = "Quota Remaining: ...";
        daysText.text = "Days Remaining: ...";


        canPlayQuotaLoad1 = true;
        canPlayQuotaLoad2 = true;
        canPlayQuotaLoad3 = true;
        canPlayQuotaLoad4 = true;   
        canPlayMoneyEarned = true;


        int currentDay = 3 - quotaData.daysLeft;
        titleText.text = "Daily Report - Day " + currentDay.ToString();
    }

    private void Start()
    {
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



        if (!gc.isPlayerDead)
        {
            if (!gc.isTimeUp)
            {

                if (timer >= 6f)
                {
                    if(canPlayQuotaLoad1)
                    {
                    canPlayQuotaLoad1 = false;
                    AudioManager.instance.playSound("QuotaLoad", false);
                    }
                    scoreText.text = "Total Value Collected: " + scoreData.score.ToString();

                }
                else if (timer >= 5f)
                {
                    if(canPlayQuotaLoad2)
                    {
                        canPlayQuotaLoad2 = false;
                    AudioManager.instance.playSound("QuotaLoad", false);
                    }
                    scoreText.text = "Total Value Collected: " + scoreData.score.ToString() + " ...selling";

                }
                else if (timer > 4f)
                {
                    if(canPlayMoneyEarned)
                    {
                        canPlayMoneyEarned = false;
                    AudioManager.instance.playSound("MoneyEarned", false);
                    }
                    scoreText.text = "Total Value Collected: Sold";

                }

                if (timer < 4f)
                {
                    if (canPlayQuotaLoad3)
                    {
                        canPlayQuotaLoad3 = false;
                    AudioManager.instance.playSound("QuotaLoad", false);
                    }
                    sellScore();
                    quotaText.text = "Quota Remaining: " + quotaData.quotaRemain.ToString();
                }

                if (timer < 2f)
                {
                    if(canPlayQuotaLoad4)
                    {
                        canPlayQuotaLoad4 = false;
                    AudioManager.instance.playSound("QuotaLoad", false);
                    }
                    daysText.text = "Days Remaining: " + quotaData.daysLeft.ToString();
                    contButton.SetActive(true);
                }


                if (quotaData.quotaRemain <= 0)
                {
                    StartCoroutine(quotaWin());
                    quotaData.quotaLevel = quotaData.quotaLevel * 1.5f;
                    quotaData.quotaRemain = Mathf.RoundToInt(50 * quotaData.quotaLevel);
                    quotaData.daysLeft = 3;
                }
            }

            else
            {
                if (timer >= 6f)
                {
                    if(canPlayQuotaLoad1)
                    {
                    canPlayQuotaLoad1 = false;
                    AudioManager.instance.playSound("QuotaLoad", false);
                    }
                    scoreText.text = "You were Transported back with nothing";
                }

                else if (timer > 4f)
                {
                    if(canPlayQuotaLoad2)
                    {
                        canPlayQuotaLoad2 = false;
                    AudioManager.instance.playSound("QuotaLoad", false);
                    }
                    scoreText.text = "Extract before the time runs out";
                 //   scoreText.text = "Get good noob";
                }

                if (timer < 4f)
                {
                    sellScore();
                    if (canPlayQuotaLoad3)
                    {
                        canPlayQuotaLoad3 = false;
                        AudioManager.instance.playSound("QuotaLoad", false);
                    }

                    quotaText.text = "Quota Remaining: " + quotaData.quotaRemain.ToString();
                }

                if (timer < 2f)
                {
                    if(canPlayQuotaLoad4)
                    {
                        canPlayQuotaLoad4   = false;
                    AudioManager.instance.playSound("QuotaLoad", false);
                    }
                    daysText.text = "Days Remaining: " + quotaData.daysLeft.ToString();
                    contButton.SetActive(true);

                }


                if (quotaData.quotaRemain <= 0)
                {
                    StartCoroutine(quotaWin());
                    quotaData.quotaLevel = quotaData.quotaLevel * 1.5f;
                    quotaData.quotaRemain = Mathf.RoundToInt(50 * quotaData.quotaLevel);
                    quotaData.daysLeft = 3;
                }
            
            
            }
        }



        else if (gc.isPlayerDead)
        {


            if (timer >= 6f)
            {
                if(canPlayQuotaLoad1)
                {
                canPlayQuotaLoad1 = false;
                AudioManager.instance.playSound("QuotaLoad", false);
                }
                scoreText.text = "Total Value Collected: " + "You have nothing to sell";

            }
            else if (timer >= 4f)
            {

                if(canPlayQuotaLoad2)
                {
                    canPlayQuotaLoad2 = false;
                AudioManager.instance.playSound("QuotaLoad", false);
                }
                scoreText.text = "Total Value Collected: " +  "X";

            }


            if (timer < 4f)
            {
                if (canPlayQuotaLoad3)
                {
                    canPlayQuotaLoad3 = false;
                    AudioManager.instance.playSound("QuotaLoad", false);
                }

                sellScore();
                quotaText.text = "Quota Remaining: " + quotaData.quotaRemain.ToString();
            }

            if (timer < 2f)
            {
                if (canPlayQuotaLoad4)
                {
                    canPlayQuotaLoad4 = false;
                    AudioManager.instance.playSound("QuotaLoad", false);
                }

                daysText.text = "Days Remaining: " + quotaData.daysLeft.ToString();
                contButton.SetActive(true);
            }


            if (quotaData.quotaRemain <= 0)
            {
                StartCoroutine(quotaWin());
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
            StartCoroutine(quotaLose());
        }
        else
        {
            lM.loadGameLevel();
        }
    }

    public void sellScore()
    {
        scoreData.itemsCollected = 0;
        quotaData.quotaRemain -= scoreData.score;
        scoreData.score = 0;
    }

    private IEnumerator quotaWin()
    {
        titleText.text = "Daily Report - Day 0";
        hubScreen.SetActive(false);
        winScreen.SetActive(true);

        yield return new WaitForSeconds(2f);

        hubScreen.SetActive(true);
        winScreen.SetActive(false);
    }

    private IEnumerator quotaLose()
    {
        hubScreen.SetActive(false);
        loseScreen.SetActive(true);
        yield return new WaitForSeconds(2f);
        lM.loadMainMenu();
    }
}
