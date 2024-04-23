using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class DeadZoneController : MonoBehaviour
{
    private CinemachineVirtualCamera vCam;
    private CinemachineComponentBase componentBase;
    private GameObject camFollowTarget;
    [SerializeField]
    private PlayerController playerController;

    void Awake()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        camFollowTarget = GameObject.FindWithTag("CameraFollowTarget");
        vCam = GetComponent<CinemachineVirtualCamera>();
        componentBase = vCam.GetCinemachineComponent(CinemachineCore.Stage.Body);
    }

    void Update()
    {

    }

    public void inDeadZone(float currentSpeed, float maxSpeed)
    {
        if (componentBase is CinemachineFramingTransposer)
        {
            var framingTransposer = componentBase as CinemachineFramingTransposer;
            if (/* check if followtarget is outside of deadzone bounds */ false)
            {
                Debug.Log("DISABLE DZ RN");
            }
        }
    }
}   
