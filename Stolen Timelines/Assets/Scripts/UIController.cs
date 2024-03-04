using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
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
    private int timer = 0;


    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        scoreText.text = "Score: " + gameController.scoreData.score.ToString();// + "\n" + "Dash Cooldown: " + playerController.playerDash.dashCooldown.ToString();
    }

    private void FixedUpdate()
    {
        timeTaken += Time.deltaTime;
        timer = Mathf.RoundToInt(timeLimit - timeTaken);

        updateTimer();

        if (timeTaken >= timeLimit)
        {
            timeTaken = 0;
            SceneManager.LoadScene("Hub");
        }
    }

    public void updateScore()
    {
        scoreText.text = "Score: " + gameController.scoreData.score.ToString();// + "\n" + "Dash Cooldown: " + playerController.seconds.ToString();
    }

    public void updateTimer()
    {
        timerText.text = "Timer: " + timer.ToString();
    }
}
