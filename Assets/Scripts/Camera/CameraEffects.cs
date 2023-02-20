using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraEffects : MonoBehaviour
{
    [Header("Target Parameters")]
    public Player player;
    public Transform[] alternativeTargets;
    public GameObject[] targetDebug;

    [Header("Target Group Parameters")]
    [Range(-1, 1)] public float playerTarget;
    [Range(-1, 1)] public float enemyTarget;
    [Range(0, 40)] public float targetingSpeed;
    public bool alternativeTarget;
    public bool debug;

    [Header("Orbital Movement")]
    [Range(0,20)] public float orbitalValue;
    [Range(0,10)] public float orbitalRecovery;

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
    private InputAction click;
    private Transform[] firstTargets;

    private CinemachineVirtualCamera vcam;
    private CinemachineOrbitalTransposer transposer;
    private CinemachineTargetGroup targetGroup;

    private void Awake()
    {
        targetGroup = GameObject.FindGameObjectWithTag("TARGET_GROUP").GetComponent<CinemachineTargetGroup>();
        vcam = GetComponent<CinemachineVirtualCamera>();
        transposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        click = new InputAction(binding: "<Mouse>/leftButton");
        click.Enable();

        firstTargets = new Transform[2];
    }

    private void Start()
    {
        cameraConditions = new bool[2];

        firstTargets[0] = targetGroup.m_Targets[0].target;
        firstTargets[1] = targetGroup.m_Targets[1].target;
    }

    private bool InitialPosition()
    {
        return transposer.m_FollowOffset == dollyZoom.zoomPositions.Item1 &&
               vcam.m_Lens.FieldOfView == dollyZoom.fieldOfView.Item1 &&
               vcam.m_Lens.NearClipPlane == dollyZoom.nearPlane.Item1;
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
            cameraConditions[1] = player.isPlayerBlocking;

            OrbitalMovement();

            TargetUpdate();

            //Making camera damping oscillate depending on player movement
            if (smoothFollowActivated) smoothFollow.ApplyMove(!player.isPlayerIdle);

            //Making camera noise oscillate depending on player movement
            if (noiseActivated) noise.ApplyMove(player.isPlayerIdle || player.isPlayerMoving);

            if (dollyZoomActivated || onGuardActivated)
            {

                if (InitialPosition() && cameraStack.Count != 0) { cameraStack.Pop(); isMoving = false; }

                if (cameraConditions[0] && !isMoving) { dollyZoom.Initialize(); cameraStack.Push(dollyZoom); isMoving = true; actualCamera = 0; }

                if (cameraConditions[1] && !isMoving) { cameraStack.Push(onGuardLinealMovement); actualCamera = 1; }

                if(cameraStack.Count != 0) cameraStack.Peek().ApplyMove(cameraConditions[actualCamera]);

            }
        }
    }

    private void OrbitalMovement()
    {
        if (player.StickSmoothDirection.x < -0.1f && !cameraConditions[1]) transposer.m_XAxis.Value -= orbitalValue * Time.deltaTime;

        if (player.StickSmoothDirection.x > 0.1f && !cameraConditions[1]) transposer.m_XAxis.Value += orbitalValue * Time.deltaTime;

        if (cameraConditions[1])
        {
            transposer.m_XAxis.Value = Mathf.Lerp(transposer.m_XAxis.Value, 0, orbitalRecovery * Time.deltaTime);
        }
    }

    private void AsignTargets(Transform[] targets)
    {
        targetGroup.m_Targets[0].target.position = Vector3.Lerp(targetGroup.m_Targets[0].target.position, targets[0].position, targetingSpeed * Time.deltaTime);
        targetGroup.m_Targets[1].target.position = Vector3.Lerp(targetGroup.m_Targets[1].target.position, targets[1].position, targetingSpeed * Time.deltaTime);
    }

    private void TargetUpdate()
    {
        if (!player.isPlayerBlocking || player.isPlayerAttacking) alternativeTarget = true; else alternativeTarget = false;

        if (alternativeTarget) AsignTargets(alternativeTargets); else AsignTargets(firstTargets);

        alternativeTargets[0].Translate(Vector3.up * playerTarget * Time.deltaTime);
        alternativeTargets[1].Translate(Vector3.up * enemyTarget * Time.deltaTime);

        if (!click.IsPressed()) { playerTarget = 0; enemyTarget = 0; }

        //Debug Targets with Spheres
        targetDebug[0].transform.position = targetGroup.m_Targets[0].target.position;
        targetDebug[1].transform.position = targetGroup.m_Targets[1].target.position;

        targetDebug[0].SetActive(debug);
        targetDebug[1].SetActive(debug);
    }
}
