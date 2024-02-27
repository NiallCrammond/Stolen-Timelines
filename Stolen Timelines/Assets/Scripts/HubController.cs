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

    private void Start()
    {
        quotaData.daysLeft -= 1;
    }

    void Update()
    {
        scoreText.text = "Score: " + scoreData.score.ToString();
        quotaText.text = "Quota: " + quotaData.quotaRemain.ToString();
        daysText.text = "Days left: " + quotaData.daysLeft.ToString();

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
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void sellScore()
    {
        scoreData.itemsCollected = 0;
        quotaData.quotaRemain -= scoreData.score;
        scoreData.score = 0;
    }
}
