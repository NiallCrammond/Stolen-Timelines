using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI scoreText;
    TextMeshProUGUI dashText;
    GameController gameController;
    PlayerController playerController;

    
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        scoreText.text = "Score: " + gameController.scoreData.score.ToString() + "\n" + "Dash Cooldown: " + playerController.playerDash.dashCooldown.ToString();
    }

    // Update is called once per frame
    public void updateScore()
    {
        scoreText.text = "Score: " + gameController.scoreData.score.ToString() + "\n" + "Dash Cooldown: " + playerController.seconds.ToString();
    }
}
