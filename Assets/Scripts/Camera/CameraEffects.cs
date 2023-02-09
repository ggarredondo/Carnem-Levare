using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [Range(0,20)] public float orbitalValue;
    [Range(0,10)] public float orbitalRecovery;


    [Header("Target Parameters")]
    public Player playerController;
    private CinemachineVirtualCamera vcam;
    private CinemachineOrbitalTransposer transposer;

    [Header("Smooth Follow Parameters")]
    public SmoothFollow smoothFollow;
    public bool smoothFollowActivated;

    [Header("Noise Parameters")]
    public Noise noise;
    public bool noiseActivated;

    [Header("Dolly Zoom Parameters")]
    public DollyZoom dollyZoom;
    public bool dollyZoomActivated;

    [Header("On Guard Parameters")]
    public LinealMovement onGuardLinealMovement;
    public bool onGuardActivated;

    private ChargePhase chargePhase;
    private float deltaTimer, chargeLimit, chargeLimitDivisor;
    private float holdingMinTime;

    private bool isMoving;
    private bool[] cameraConditions;
    private int actualCamera;

    private Stack<CameraMovement> cameraStack = new();

    private void Awake()
    {
        vcam = GetComponentInChildren<CinemachineVirtualCamera>();
        transposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        cameraConditions = new bool[2];
    }

    // Charge Attack provisional functions ////////////////
    public void SetChargeValues(ChargePhase _chargePhase, float _deltaTimer)
    {
        chargePhase = _chargePhase;
        deltaTimer = _deltaTimer;
    }

    public void SetChargeValues(ChargePhase _chargePhase, float _deltaTimer, float _chargeLimit, float _chargeLimitDivisor)
    {
        chargePhase = _chargePhase;
        deltaTimer = _deltaTimer;
        chargeLimit = _chargeLimit;
        chargeLimitDivisor = _chargeLimitDivisor;
    }
    ////////////////////////////////////////////////////////

    public void Initialized()
    {
        holdingMinTime = chargeLimit / chargeLimitDivisor;
        dollyZoom.aceleration.Item1 = 1 / (chargeLimit - holdingMinTime);
    }

    private void LateUpdate()
    {
        if (Time.timeScale > 0f)
        {
            cameraConditions[0] = (chargePhase == ChargePhase.performing) && (deltaTimer >= holdingMinTime) && (deltaTimer <= chargeLimit);
            cameraConditions[1] = playerController.getIsBlocking;

            OrbitalMovement();

            //Making camera damping oscillate depending on player movement
            if (smoothFollowActivated) smoothFollow.ApplyMove(playerController.getIsMoving);

            //Making camera noise oscillate depending on player movement
            if (noiseActivated) noise.ApplyMove(playerController.getIsMovement);

            if (dollyZoomActivated || onGuardActivated)
            {

                if (transposer.m_FollowOffset == dollyZoom.zoomPositions.Item1 && cameraStack.Count != 0) { cameraStack.Pop(); isMoving = false; }

                if (cameraConditions[0] && !isMoving) { dollyZoom.Initialize(); cameraStack.Push(dollyZoom); isMoving = true; actualCamera = 0; }

                if (cameraConditions[1] && !isMoving) { cameraStack.Push(onGuardLinealMovement); actualCamera = 1; }

                if(cameraStack.Count != 0) cameraStack.Peek().ApplyMove(cameraConditions[actualCamera]);

            }
        }
    }

    private void OrbitalMovement()
    {
        if (playerController.getDirectionX < -0.1f && !cameraConditions[1]) transposer.m_XAxis.Value -= orbitalValue * Time.deltaTime;

        if (playerController.getDirectionX > 0.1f && !cameraConditions[1]) transposer.m_XAxis.Value += orbitalValue * Time.deltaTime;

        if (cameraConditions[1])
        {
            transposer.m_XAxis.Value = Mathf.Lerp(transposer.m_XAxis.Value, 0, orbitalRecovery * Time.deltaTime);
        }
    }
}
