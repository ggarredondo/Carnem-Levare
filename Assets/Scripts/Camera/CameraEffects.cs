using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private GameObject[] targetDebug;

    [Header("Target Group Parameters")]
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

    private Queue<CameraMovement> cameraStack = new();
    private Dictionary<TypeCameraMovement, CameraMovement> cameraDictionary = new();

    private CinemachineVirtualCamera vcam;
    private CinemachineOrbitalTransposer orbitalTransposer;
    private CinemachineTargetGroup targetGroup;

    private void Awake()
    {
        alternativeTargets = new Transform[2];

        vcam = GetComponent<CinemachineVirtualCamera>();
        orbitalTransposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        cameraEffects.ForEach(effect => cameraDictionary.Add(effect.ID, effect));
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

        cameraEffects.ForEach(movement => movement.Initialize(ref vcam));

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
        player.StateMachine.BlockingState.OnEnter += () =>
        {
            cameraDictionary[TypeCameraMovement.LINEAL_MOVE].ApplyCondition = true;
            isBlocking = true;
        };

        player.StateMachine.BlockingState.OnExit += () =>
        {
            cameraDictionary[TypeCameraMovement.LINEAL_MOVE].ApplyCondition = false;
            isBlocking = false;
        };

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

        player.StateMachine.WalkingState.OnEnter += () =>
        {
            cameraDictionary[TypeCameraMovement.SMOOTH_FOLLOW].ApplyCondition = true;
            cameraDictionary[TypeCameraMovement.NOISE].ApplyCondition = true;
        };

        player.StateMachine.WalkingState.OnExit += () =>
        {
            cameraDictionary[TypeCameraMovement.SMOOTH_FOLLOW].ApplyCondition = false;
            cameraDictionary[TypeCameraMovement.NOISE].ApplyCondition = false;
        };

        enemy.StateMachine.WalkingState.OnEnter += () => changeEnemyTargets = true;
        enemy.StateMachine.WalkingState.OnExit += () => changeEnemyTargets = false;
    }

    private void LateUpdate()
    {
        if (Time.timeScale > 0f)
        {
            if(orbitalTransposer != null) OrbitalMovement();

            //Change the camera targets to create more hitting impact
            if(alternativeTargets[0] != null && alternativeTargets[1] != null) TargetUpdate();

            //Apply the camera movement to each effect depending of his condition
            foreach (CameraMovement movement in cameraEffects)
            {
                if(!movement.Stackable) movement.ApplyMove();
                else
                {
                    cameraStack.TryPeek(out CameraMovement peek);
                    
                    if(!movement.ApplyCondition) movement.ReturnInitialPosition();

                    if (movement.ApplyCondition && peek != movement)
                    {
                        movement.UpdateInitialPosition();
                        cameraStack.Enqueue(movement);
                    }
                }
            }

            if(cameraStack.Count > 3) cameraStack.Dequeue();

            if(cameraStack.Count != 0) cameraStack.Peek().ApplyMove();
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
