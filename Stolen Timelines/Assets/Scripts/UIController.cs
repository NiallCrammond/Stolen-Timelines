using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI scoreText;

    GameController gameController;

    
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        scoreText.text = "Score: " + gameController.scoreData.score.ToString();
    }

    // Update is called once per frame
    public void updateScore()
    {
        scoreText.text = "Score: " + gameController.scoreData.score.ToString();
    }
}
