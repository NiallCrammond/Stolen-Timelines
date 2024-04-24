using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Runtime.CompilerServices;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineVirtualCamera[] allVirtualCamers;

    [Header("Controls for lerping the Y damping during player fall")]
    [SerializeField] private float fallPanAmount = 0.25f;
    [SerializeField] private float fallPanTime = 0.35f;
    public float fallSpeedYDampingChnageThreshold = -15f;

    [Header("Controls for deadzone pan speed upon landing")]
    [SerializeField] private float deadZonePanTime = 0.35f;

    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }
    public bool IsDeadZoneChanging { get; private set; }

    private Coroutine lerpYPanCoroutine;
    private Coroutine deadZoneCoroutine;

    private CinemachineVirtualCamera currentCamera;
    private CinemachineFramingTransposer framingTransposer;

    private float normYPanAmount;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        for (int i = 0; i < allVirtualCamers.Length; i++)
        {
            if (allVirtualCamers[i].enabled)
            {
                currentCamera = allVirtualCamers[i];

                framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }

        //set Ydamping
        normYPanAmount = framingTransposer.m_YDamping;
    }

    #region Lerp the Y Damping

    public void LerpYDamping(bool isPlayerFalling)
    {
        lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;

        //grab start damping amount
        float startDampAmount = framingTransposer.m_YDamping;
        float endDampAmount = 0f;

        //determine end damping amount
        if (isPlayerFalling)
        {
            endDampAmount = fallPanAmount;
            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = normYPanAmount;
        }

        //lerp pan amount
        float elapsedTime = 0f;
        while(elapsedTime < fallPanTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / fallPanTime));
            framingTransposer.m_YDamping = lerpedPanAmount;

            yield return null;
        }

        IsLerpingYDamping = false;
    }

    #endregion

    #region Deadzone changer

    public void ChangeDeadZoneOnLanding(bool newGrounded)
    {
        deadZoneCoroutine = StartCoroutine(deadZoneChange(newGrounded));
    }

    private IEnumerator deadZoneChange(bool newGrounded)
    {
        IsDeadZoneChanging = true;

        //get start vals for deadzone
        float startHeight = 0.5f;
        float targetHeight = 0f;

        //center Y deadzone (centre cam) upon landing on ground
        if (newGrounded)
        {
            framingTransposer.m_DeadZoneHeight = targetHeight;
            framingTransposer.m_SoftZoneHeight = startHeight / 2;
        }
        else
        {
            framingTransposer.m_DeadZoneHeight = startHeight;
        }

        /*lerp pan amount
        float elapsedTime = 0f;
        while (elapsedTime < deadZonePanTime)
        {
            elapsedTime += Time.deltaTime;

            float deadZonePanAmount = Mathf.Lerp(targetHeight, startHeight, (elapsedTime / deadZonePanTime));
            framingTransposer.m_DeadZoneHeight = deadZonePanAmount;

            yield return null;

        }*/
        yield return null;

        IsDeadZoneChanging = false;
    }
    #endregion
}
