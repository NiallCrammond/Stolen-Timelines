using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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


    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        scoreText.text = "Score: " + gameController.scoreData.score.ToString();// + "\n" + "Dash Cooldown: " + playerController.playerDash.dashCooldown.ToString();


    }

    private void FixedUpdate()
    {
        timeTaken += Time.deltaTime;
        timer = (timeLimit - timeTaken);

        updateTimer();

        if (timeTaken >= timeLimit)
        {
            timeTaken = 0;
            SceneManager.LoadScene("Hub");
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
