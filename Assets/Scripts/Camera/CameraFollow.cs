using UnityEngine;
using Cinemachine;
using System;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Objects")]
    public PlayerController playerController;
    private CinemachineVirtualCamera vcam;

    [Header("Follow Parameters")]
    public Tuple<float> cameraAceleration;
    public Tuple<float> damping;
    public bool cameraRotation;

    private float reduceDamping;
    private CinemachineTransposer transposer;

    [Header("Noise Parameters")]
    public Tuple<float> noiseAceleration;
    public Tuple<float> frequency;
    public Tuple<float> amplitude;
    public bool cameraNoise;

    private float reduceAmplitude, reduceFrequency;
    private CinemachineBasicMultiChannelPerlin noiseTransposer;

    [Header("Dolly Zoom Parameters")]
    public Tuple<float> zoomAceleration;
    public Tuple<float> fieldOfView;
    public Vector3 dollyOffsetVariation;
    public bool cameraMovement;

    private float reduceFOV, reduceDollyZoom;
    private Tuple<Vector3> dollyZoomPositions;

    [Header("On Guard Parameters")]
    public Tuple<float> onGuardAceleration;
    public Vector3 onGuardOffsetVariation;
    private float reduceOnGuard;

    private Tuple<Vector3> onGuardPositions;

    [NonSerialized] public Move currentMove;
    private float holdingMinTime;

    private bool isMoving;
    private int actualCameraMovementFunction;

    Dictionary<int, Action> functionsMap = new Dictionary<int, Action>();

    private void Awake()
    {
        vcam = GetComponentInChildren<CinemachineVirtualCamera>();
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
        noiseTransposer = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        functionsMap.Add(1, () => DollyZoom());
        functionsMap.Add(2, () => BlockingZoom());
    }

    private void Start()
    {
        currentMove = playerController.rightNormalSlot;

        dollyZoomPositions.Item1 = transposer.m_FollowOffset;
        dollyZoomPositions.Item2 = dollyZoomPositions.Item1 + dollyOffsetVariation;

        onGuardPositions.Item1 = transposer.m_FollowOffset;
        onGuardPositions.Item2 = onGuardPositions.Item1 + onGuardOffsetVariation;
    }

    public void Initialized()
    {
        holdingMinTime = currentMove.getChargeLimit / currentMove.chargeLimitDivisor;
        zoomAceleration.Item1 = 1 / (currentMove.getChargeLimit - holdingMinTime);
    }

    private void LateUpdate()
    {
        if (Time.timeScale > 0f)
        {
            //Making camera damping oscillate depending on player movement
            if (cameraRotation)
            {
                transposer.m_YawDamping = CameraUtilities.OscillateParameter(playerController.getIsMoving, cameraAceleration, ref reduceDamping, damping, CameraUtilities.Exponential);
            }

            //Making different camera movements depending on the movement of the player
            if (cameraMovement)
            {
                if (transposer.m_FollowOffset == dollyZoomPositions.Item1) isMoving = false;

                if (currentMove.getChargePhase == Move.ChargePhase.performing && currentMove.getDeltaTimer >= holdingMinTime && currentMove.getDeltaTimer <= currentMove.getChargeLimit && !isMoving)
                {
                    actualCameraMovementFunction = 1;
                    isMoving = true;
                }

                if (playerController.getIsBlocking && !isMoving)
                {
                    actualCameraMovementFunction = 2;
                }

                if (actualCameraMovementFunction != 0)
                    functionsMap[actualCameraMovementFunction]();
            }

            //Making camera noise oscillate depending on player movement
            if (cameraNoise)
            {
                noiseTransposer.m_FrequencyGain = CameraUtilities.OscillateParameter(playerController.getIsMovement, noiseAceleration, ref reduceFrequency, frequency, CameraUtilities.Exponential);
                noiseTransposer.m_AmplitudeGain = CameraUtilities.OscillateParameter(playerController.getIsMovement, noiseAceleration, ref reduceAmplitude, amplitude, CameraUtilities.Exponential);
            }
        }
    }

    private void DollyZoom()
    {
        transposer.m_FollowOffset = CameraUtilities.Movement(currentMove.getChargePhase == Move.ChargePhase.performing && currentMove.getDeltaTimer >= holdingMinTime && currentMove.getDeltaTimer <= currentMove.getChargeLimit,
                                                             zoomAceleration, ref reduceDollyZoom, new Vector3[] { dollyZoomPositions.Item1, dollyZoomPositions.Item2 }, CameraUtilities.LinearBezierCurve);

        vcam.m_Lens.FieldOfView = CameraUtilities.OscillateParameter(currentMove.getChargePhase == Move.ChargePhase.performing && currentMove.getDeltaTimer >= holdingMinTime && currentMove.getDeltaTimer <= currentMove.getChargeLimit,
                                                                     zoomAceleration, ref reduceFOV, fieldOfView, Mathf.Sin);
    }

    private void BlockingZoom()
    {
        transposer.m_FollowOffset = CameraUtilities.Movement(playerController.getIsBlocking, onGuardAceleration, ref reduceOnGuard, new Vector3[] { onGuardPositions.Item1, onGuardPositions.Item2 }, CameraUtilities.LinearBezierCurve);
    }
}