using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private GameObject[] targetDebug;

    [Header("Target Group Parameters")]
    [SerializeField] private bool alternativeTargeting;
    [Range(0, 40)] [SerializeField] private float targetingSpeed;
    [SerializeField] private bool debug;

    [Header("Orbital Movement")]
    [Range(0,20)] [SerializeField] private float orbitalValue;
    [Range(0,10)] [SerializeField] private float orbitalRecovery;

    [Header("Effects")]
    [SerializeField] private List<CameraMovement> cameraEffects;

    //PRIVATE
    private Player player;
    private Enemy enemy;
    private Transform[] alternativeTargets;
    private bool changePlayerTargets, changeEnemyTargets, isBlocking, isMoving;

    private CinemachineVirtualCamera vcam;
    private CinemachineOrbitalTransposer orbitalTransposer;
    private CinemachineTargetGroup targetGroup;

    private void Awake()
    {
        alternativeTargets = new Transform[2];

        vcam = GetComponent<CinemachineVirtualCamera>();
        orbitalTransposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

        cameraEffects.ForEach(movement => { movement.Initialize(ref vcam); movement.UpdateCondition(ref player); });

        UpdateCameraConditions();
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

    private void UpdateCameraConditions()
    {
        player.StateMachine.BlockingState.OnEnter += () => isBlocking = true;

        player.StateMachine.BlockingState.OnExit += () => isBlocking = false;

        player.StateMachine.MoveState.OnEnter += () =>
        {
            changePlayerTargets = true;
            isMoving = true;
        };

        player.StateMachine.MoveState.OnExit += () =>
        {
            changePlayerTargets = false;
            isMoving = false;
        };

        enemy.StateMachine.WalkingState.OnEnter += () => changeEnemyTargets = true;
        enemy.StateMachine.WalkingState.OnExit += () => changeEnemyTargets = false;
    }

    private void LateUpdate()
    {
        if (Time.timeScale > 0f)
        {
            if(orbitalTransposer != null) OrbitalMovement();

            if(alternativeTargets[0] != null && alternativeTargets[1] != null && alternativeTargeting) TargetUpdate();

            cameraEffects.ForEach(movement => movement.ApplyMove());
        }
    }
 
    private void OrbitalMovement()
    {
        if (player.Controller.MovementVector != new Vector2(0,0) && !isBlocking && !isMoving) 
            orbitalTransposer.m_XAxis.Value += player.Controller.MovementVector.x * orbitalValue * Time.deltaTime;

        if (isBlocking) 
            orbitalTransposer.m_XAxis.Value = Mathf.Lerp(orbitalTransposer.m_XAxis.Value, 0, orbitalRecovery * Time.deltaTime);

        orbitalTransposer.m_RecenterToTargetHeading.m_enabled = player.Controller.MovementVector != new Vector2(0, 0) && !isBlocking && !isMoving;
    }
    
    private void AsignAlternativeTarget(int index)
    {
        targetGroup.m_Targets[index].target.position = Vector3.Lerp(targetGroup.m_Targets[index].target.position, alternativeTargets[index].position, targetingSpeed * Time.deltaTime);
    }

    private void TargetUpdate()
    {
        if (changePlayerTargets) AsignAlternativeTarget(0);
        if(changeEnemyTargets) AsignAlternativeTarget(1);

        //Debug Targets with Spheres
        targetDebug[0].transform.position = targetGroup.m_Targets[0].target.position;
        targetDebug[1].transform.position = targetGroup.m_Targets[1].target.position;

        targetDebug[0].SetActive(debug);
        targetDebug[1].SetActive(debug);
    }  
}
