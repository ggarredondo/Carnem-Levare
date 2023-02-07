using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [Range(0,1)] public float orbitalValue;

    [Header("Target Parameters")]
    public PlayerController playerController;
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

    [NonSerialized] public Move currentMove;
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

    private void Start()
    {
        currentMove = playerController.rightNormalSlot;
    }

    public void Initialized()
    {
        holdingMinTime = currentMove.getChargeLimit / currentMove.chargeLimitDivisor;
        dollyZoom.aceleration.Item1 = 1 / (currentMove.getChargeLimit - holdingMinTime);
    }

    private void LateUpdate()
    {
        if (Time.timeScale > 0f)
        {
            cameraConditions[0] = currentMove.getChargePhase == Move.ChargePhase.performing && currentMove.getDeltaTimer >= holdingMinTime && currentMove.getDeltaTimer <= currentMove.getChargeLimit;
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
        if (playerController.getDirectionX < -0.1 && !cameraConditions[1]) transposer.m_XAxis.Value -= orbitalValue;

        if (playerController.getDirectionX > 0.1 && !cameraConditions[1]) transposer.m_XAxis.Value += orbitalValue;

        if (cameraConditions[1])
        {
            transposer.m_XAxis.Value = Mathf.Lerp(transposer.m_XAxis.Value, 0, orbitalValue);
        }
    }
}