using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DemoPage : MonoBehaviour
{

    float timeElapsed;
    public float waitTime;
    bool canContinue = false;
    public QuotaData quotaData;
    public TextMeshProUGUI quotaLevel;
    public TextMeshProUGUI quotaToCollect;
    public TextMeshProUGUI daysLeft;
    public TextMeshProUGUI continueText;
   
    // Start is called before the first frame update
    void Start()
    {
        quotaLevel.text += quotaData.quotaLevel;
        quotaToCollect.text += quotaData.quotaRemain;
        daysLeft.text += quotaData.daysLeft;

    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > waitTime && !canContinue)
        {
            canContinue=true;
            continueText.gameObject.SetActive(true);
        }

        if (timeElapsed > waitTime && Input.anyKeyDown)
        {
            SceneManager.LoadScene("BuildSubmissionV1");
        }
    }
}
