using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ShakeOnHit : MonoBehaviour
{

    public float hitAmplitudeGain = 2, hitFrequencyGain = 2, shakeTime = 1;

    CinemachineVirtualCamera vcam;
    CinemachineBasicMultiChannelPerlin noisePerlin;

    bool isShaking = false;
    float shakeTimeElapsed = 0;

    void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        noisePerlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeIfPlayerHit(int health)
    {
        StartShake();
    }

    public void StartShake()
    {
        //Debug.Log("Player hit, shake cam");
        noisePerlin.m_AmplitudeGain = hitAmplitudeGain;
        noisePerlin.m_FrequencyGain = hitFrequencyGain;
        isShaking = true;
        shakeTimeElapsed = 0f;
    }

    public void StopShake()
    {
        noisePerlin.m_AmplitudeGain = 0;
        noisePerlin.m_FrequencyGain = 0;
        isShaking = false;
        shakeTimeElapsed = 0f;

    }

    void Update()
    {
        shakeTimeElapsed += Time.deltaTime;

        if(shakeTimeElapsed > shakeTime)
        {
            StopShake();
        }
    }
}
