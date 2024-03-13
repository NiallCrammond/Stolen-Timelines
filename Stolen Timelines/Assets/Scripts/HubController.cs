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

    private float timer;

    private void Awake()
    {
        quotaData.daysLeft -= 1;
        timer = 8f;

        scoreText.text = "Total Value Collected: ...";
        quotaText.text = "Quota Remaining: ...";
        daysText.text = "Days Remaining: ...";

        int currentDay = 3 - quotaData.daysLeft;
        titleText.text = "Daily Report - Day " + currentDay.ToString();
    }

    void Update()
    {
        timer -= Time.deltaTime;


        if(timer >= 6f)
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
            quotaData.quotaRemain = Mathf.RoundToInt(200 * quotaData.quotaLevel);

            quotaData.daysLeft = 3;
        }

    }

    public void goAgain()
    {
        if (quotaData.daysLeft <= 0 && quotaData.quotaRemain > 0)
        {
            quotaData = new QuotaData();
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
