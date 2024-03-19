using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
public class UIController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI timerText;
    TextMeshProUGUI dashText;
    GameController gameController;
    PlayerController playerController;

    [SerializeField]
    private float timeLimit = 60f;
    private float timeTaken = 0f;
    private float timer = 0;
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private Image dashBar;

    bool timeUp;


    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        scoreText.text = "Score: " + gameController.scoreData.score.ToString();// + "\n" + "Dash Cooldown: " + playerController.playerDash.dashCooldown.ToString();
        timeUp = false;

    }

    private void FixedUpdate()
    {
        if(!timeUp)
        {

        timeTaken += Time.deltaTime;

        updateTimer();
        timer = (timeLimit - timeTaken);
        }

        if (timeTaken >= timeLimit)
        {
            timeUp = true;
            Mathf.RoundToInt(timer);
           gameController.scoreData.score = 0;
            gameController.scoreData.itemsCollected = 0;
            gameController.isTimeUp = true;
            GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().loadHub();
        }
    }

    public void updateScore()
    {
        scoreText.text = "$" + gameController.scoreData.score.ToString();// + "\n" + "Dash Cooldown: " + playerController.seconds.ToString();
    }

    public void updateTimer()
    {
        timerText.text = "Timer: " + timer.ToString("F3");
    }

    public void updateHealthBar(int health)
    {
        healthBar.fillAmount = health / 100f;
    }

    public void updateDashBar(float dashCDTimer, float dashCD)
    {
        dashBar.fillAmount = (dashCDTimer / dashCD);
    }

   public void addTime(float time)
    {
        Debug.Log("Time added");
        timeLimit += (int)time;
    }
}
