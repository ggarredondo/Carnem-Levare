using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraEffects : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private Player player;
    [SerializeField] private Transform[] alternativeTargets;
    [SerializeField] private GameObject[] targetDebug;

    [Header("Target Group Parameters")]
    [Range(-1, 1)] [SerializeField] private float playerTarget;
    [Range(-1, 1)] [SerializeField] private float enemyTarget;
    [Range(0, 40)] [SerializeField] private float targetingSpeed;
    [SerializeField] private bool alternativeTarget;
    [SerializeField] private bool debug;

    [Header("Orbital Movement")]
    [Range(0,20)] [SerializeField] private float orbitalValue;
    [Range(0,10)] [SerializeField] private float orbitalRecovery;

    [Header("Effects")]
    public bool smoothFollowActivated;
    [ConditionalField("smoothFollowActivated")] public SmoothFollow smoothFollow;

    public bool noiseActivated;
    [ConditionalField("noiseActivated")] public Noise noise;

    public bool dollyZoomActivated;
    [ConditionalField("dollyZoomActivated")] public DollyZoom dollyZoom;

    public bool onGuardActivated;
    [ConditionalField("onGuardActivated")] public LinealMovement onGuardLinealMovement;

    //PRIVATE
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
    private CinemachineOrbitalTransposer orbitalTransposer;
    private CinemachineTargetGroup targetGroup;

    private void Awake()
    {
        targetGroup = GameObject.FindGameObjectWithTag("TARGET_GROUP").GetComponent<CinemachineTargetGroup>();
        vcam = GetComponent<CinemachineVirtualCamera>();

        orbitalTransposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();

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
        return orbitalTransposer.m_FollowOffset == dollyZoom.zoomPositions.Item1 &&
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

            if(orbitalTransposer != null) OrbitalMovement();

            if(alternativeTargets.GetLength(0) != 0) TargetUpdate();

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
        if (Mathf.Abs(player.StickSmoothDirection.x) > 0.1f && player.isPlayerMoving && !cameraConditions[1]) 
            orbitalTransposer.m_XAxis.Value += Mathf.Sign(player.StickSmoothDirection.x) * orbitalValue * Time.deltaTime;

        if (cameraConditions[1]) orbitalTransposer.m_XAxis.Value = Mathf.Lerp(orbitalTransposer.m_XAxis.Value, 0, orbitalRecovery * Time.deltaTime);

        orbitalTransposer.m_RecenterToTargetHeading.m_enabled = Mathf.Abs(player.StickSmoothDirection.x) > 0.1f && !cameraConditions[1];
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
