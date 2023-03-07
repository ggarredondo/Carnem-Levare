using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [Header("Requirements")]
    private Player player;
    private Transform[] alternativeTargets;
    [SerializeField] private GameObject[] targetDebug;

    [Header("Target Group Parameters")]
    [Range(0, 40)] [SerializeField] private float targetingSpeed;
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

    private CinemachineVirtualCamera vcam;
    private CinemachineOrbitalTransposer orbitalTransposer;
    private CinemachineTargetGroup targetGroup;

    private void Awake()
    {
        alternativeTargets = new Transform[2];

        vcam = GetComponent<CinemachineVirtualCamera>();
        orbitalTransposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        cameraConditions = new bool[2];
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void InitializeTargetGroup(Transform playerTarget, Transform enemyTarget)
    {
        targetGroup = GameObject.FindGameObjectWithTag("TARGET_GROUP").GetComponent<CinemachineTargetGroup>();

        targetGroup.m_Targets[0].target = playerTarget;
        targetGroup.m_Targets[1].target = enemyTarget;
    }

    public void InitializeTargets(Transform playerAlternative, Transform enemyAlternative)
    {
        alternativeTargets[0] = playerAlternative;
        alternativeTargets[1] = enemyAlternative;
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
            cameraConditions[1] = player.IsBlocking;

            if(orbitalTransposer != null) OrbitalMovement();

            if(alternativeTargets[0] != null && alternativeTargets[1] != null) TargetUpdate();

            //Making camera damping oscillate depending on player movement
            if (smoothFollowActivated) smoothFollow.ApplyMove(!player.IsIdle);

            //Making camera noise oscillate depending on player movement
            if (noiseActivated) noise.ApplyMove(player.IsIdle || player.IsMoving);

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
        if (Mathf.Abs(player.Direction.x) > 0.1f && player.IsMoving && !cameraConditions[1]) 
            orbitalTransposer.m_XAxis.Value += Mathf.Sign(player.Direction.x) * orbitalValue * Time.deltaTime;

        if (cameraConditions[1]) orbitalTransposer.m_XAxis.Value = Mathf.Lerp(orbitalTransposer.m_XAxis.Value, 0, orbitalRecovery * Time.deltaTime);

        orbitalTransposer.m_RecenterToTargetHeading.m_enabled = Mathf.Abs(player.Direction.x) > 0.1f && player.IsMoving && !cameraConditions[1];
    }

    private void AsignTargets(Transform[] targets)
    {
        targetGroup.m_Targets[0].target.position = Vector3.Lerp(targetGroup.m_Targets[0].target.position, targets[0].position, targetingSpeed * Time.deltaTime);
        targetGroup.m_Targets[1].target.position = Vector3.Lerp(targetGroup.m_Targets[1].target.position, targets[1].position, targetingSpeed * Time.deltaTime);
    }

    private void TargetUpdate()
    {
        if (!player.IsBlocking || player.IsAttacking) AsignTargets(alternativeTargets);

        //Debug Targets with Spheres
        targetDebug[0].transform.position = targetGroup.m_Targets[0].target.position;
        targetDebug[1].transform.position = targetGroup.m_Targets[1].target.position;

        targetDebug[0].SetActive(debug);
        targetDebug[1].SetActive(debug);
    }
}
