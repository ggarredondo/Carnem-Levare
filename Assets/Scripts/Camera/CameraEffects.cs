using Cinemachine;
using System;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [Header("Target Parameters")]
    public PlayerController playerController;
    public CinemachineVirtualCamera vcam;

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

    private void Awake()
    {
        smoothFollow = new(vcam);
        noise = new(vcam);
        dollyZoom = new(vcam);
        onGuardLinealMovement = new(vcam);
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
            //Making camera damping oscillate depending on player movement
            if (smoothFollowActivated) smoothFollow.ApplyMove(playerController.getIsMoving);

            //Making camera noise oscillate depending on player movement
            if (noiseActivated) noise.ApplyMove(playerController.getIsMovement);

            //Making camera dollyZoom when player hold an attack
            if (dollyZoomActivated) dollyZoom.ApplyMove(currentMove.getChargePhase == Move.ChargePhase.performing && currentMove.getDeltaTimer >= holdingMinTime && currentMove.getDeltaTimer <= currentMove.getChargeLimit);

            //Making lineal movement when player is blocking
            if (onGuardLinealMovement) onGuardLinealMovement.ApplyMove(playerController.getIsBlocking);
        }
    }
}
