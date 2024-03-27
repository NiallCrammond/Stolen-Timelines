using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.Controls;
using System;
using UnityEngine.SceneManagement;


public class StartScreenUI : MonoBehaviour
{

    public TextMeshProUGUI quotaLevel;
    public TextMeshProUGUI quotaToCollect;
    public TextMeshProUGUI daysToQuota;

    public QuotaData quotaData;
    private GameController gc;

   public LevelManager levelManager;

    float timer = 0;
    private void Awake()
    {
        timer = 0;
        gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        quotaLevel.text = "Quota Level: " + quotaData.quotaLevel;
        quotaToCollect.text = "Quota to Collect: " + quotaData.quotaRemain;
        daysToQuota.text = "Days Remaining: " + quotaData.daysLeft;

        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(Input.anyKey &&  gc.level.isDone && timer>5)
        {
            gc.level.allowSceneActivation = true;
            SceneManager.LoadScene("BuildSubmissionV1");

        }
    }
}
