using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [Header("Target Parameters")]
    public PlayerController playerController;
    private CinemachineVirtualCamera vcam;
    private CinemachineTransposer transposer;

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

    private Vector3 initialPosition;
    private bool isMoving;
    private bool[] cameraConditions;
    private int actualCamera;
    Dictionary<int, CameraMovement> cameraMap = new();

    Stack<CameraMovement> cameraStack = new();

    private void Awake()
    {
        vcam = GetComponentInChildren<CinemachineVirtualCamera>();
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();

        cameraMap.Add(1, dollyZoom);
        cameraMap.Add(2, onGuardLinealMovement);

        cameraConditions = new bool[2];
    }

    private void Start()
    {
        initialPosition = transposer.m_FollowOffset;
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
            //Making camera damping oscillate depending on player movement
            if (smoothFollowActivated) smoothFollow.ApplyMove(playerController.getIsMoving);

            //Making camera noise oscillate depending on player movement
            if (noiseActivated) noise.ApplyMove(playerController.getIsMovement);

            if (dollyZoomActivated || onGuardActivated)
            {
                cameraConditions[0] = currentMove.getChargePhase == Move.ChargePhase.performing && currentMove.getDeltaTimer >= holdingMinTime && currentMove.getDeltaTimer <= currentMove.getChargeLimit;

                cameraConditions[1] = playerController.getIsBlocking;

                if (transposer.m_FollowOffset == dollyZoom.zoomPositions.Item1 && cameraStack.Count != 0) { cameraStack.Pop(); isMoving = false; }

                if (cameraConditions[0] && !isMoving) { dollyZoom.Initialize(); cameraStack.Push(dollyZoom); isMoving = true; actualCamera = 0; }

                if (cameraConditions[1] && !isMoving) { cameraStack.Push(onGuardLinealMovement); actualCamera = 1; }

                if(cameraStack.Count != 0) cameraStack.Peek().ApplyMove(cameraConditions[actualCamera]);

            }
        }
    }
}
