using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public ScoreData scoreData;

    private void Awake()
    {
        scoreData.score = 0;
        scoreData.itemsCollected = 0;
    }
    public void increaseScore()
    {
        scoreData.score += 10;
        scoreData.itemsCollected++;
    }

}
