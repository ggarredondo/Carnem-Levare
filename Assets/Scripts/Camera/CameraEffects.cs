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
    [SerializeField] private CameraMovement[] cameraEffects;

    //PRIVATE
    private Player player;
    private Enemy enemy;
    private Transform[] alternativeTargets;

    private bool[] cameraConditions;
    private Queue<CameraMovement> cameraStack = new();

    private CinemachineVirtualCamera vcam;
    private CinemachineOrbitalTransposer orbitalTransposer;
    private CinemachineTargetGroup targetGroup;

    private void Awake()
    {
        alternativeTargets = new Transform[2];

        vcam = GetComponent<CinemachineVirtualCamera>();
        orbitalTransposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        cameraConditions = new bool[Enum.GetNames(typeof(TypeCameraMovement)).Length];
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();

        foreach(CameraMovement movement in cameraEffects)
        {
            movement.Initialize(vcam);
        }
    }

    #region Public

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

    #endregion

    #region Private

    /// <summary>
    /// Update the conditions that enable/disable the camera effects
    /// </summary>
    private void UpdateCameraConditions()
    {
        foreach (CameraMovement movement in cameraEffects)
        {
            switch (movement.ID)
            {
                case TypeCameraMovement.SMOOTH_FOLLOW:
                    cameraConditions[(int)TypeCameraMovement.SMOOTH_FOLLOW] = !player.IsIdle;
                    break;

                case TypeCameraMovement.LINEAL_MOVE:
                    cameraConditions[(int)TypeCameraMovement.LINEAL_MOVE] = player.IsBlocking;
                    break;

                case TypeCameraMovement.NOISE:
                    cameraConditions[(int)TypeCameraMovement.NOISE] = player.IsIdle || player.IsMoving;
                    break;

                case TypeCameraMovement.DOLLY_ZOOM:
                    cameraConditions[(int)TypeCameraMovement.DOLLY_ZOOM] = player.IsAttacking;
                    break;
            }
        }
    }

    private void LateUpdate()
    {
        if (Time.timeScale > 0f)
        {
            if(orbitalTransposer != null) OrbitalMovement();

            //Change the camera targets to create more hitting impact
            if(alternativeTargets[0] != null && alternativeTargets[1] != null) TargetUpdate();

            UpdateCameraConditions();

            //Apply the camera movement to each effect depending of his condition
            foreach (CameraMovement movement in cameraEffects)
            {
                if(!movement.Stackable) movement.ApplyMove(cameraConditions[(int)movement.ID]);
                else
                {
                    cameraStack.TryPeek(out CameraMovement peek);
                    
                    if(!cameraConditions[(int)movement.ID]) movement.ReturnInitialPosition();

                    if (cameraConditions[(int)movement.ID] && peek != movement)
                    {
                        movement.UpdateInitialPosition();
                        cameraStack.Enqueue(movement);
                    }
                }
            }

            if(cameraStack.Count > cameraConditions.GetLength(0)) cameraStack.Dequeue();

            if(cameraStack.Count != 0) cameraStack.Peek().ApplyMove(cameraConditions[(int) cameraStack.Peek().ID]);
        }
    }

    /// <summary>
    /// Camera orbital movement between the player and the enemy
    /// </summary>
    private void OrbitalMovement()
    {
        if (Mathf.Abs(player.Direction.x) > 0.1f && player.IsMoving && !cameraConditions[1]) 
            orbitalTransposer.m_XAxis.Value += Mathf.Sign(player.Direction.x) * orbitalValue * Time.deltaTime;

        if (player.IsBlocking) orbitalTransposer.m_XAxis.Value = Mathf.Lerp(orbitalTransposer.m_XAxis.Value, 0, orbitalRecovery * Time.deltaTime);

        orbitalTransposer.m_RecenterToTargetHeading.m_enabled = Mathf.Abs(player.Direction.x) > 0.1f && player.IsMoving && !cameraConditions[1];
    }

    /// <summary>
    /// Change the target group position to the alternative target position
    /// </summary>
    /// <param name="index">0 (player target), 1 (enemy target)</param>
    private void AsignAlternativeTarget(int index)
    {
        targetGroup.m_Targets[index].target.position = Vector3.Lerp(targetGroup.m_Targets[index].target.position, alternativeTargets[index].position, targetingSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Change the target of enemy and player depending on a condition
    /// </summary>
    private void TargetUpdate()
    {
        if (!player.IsBlocking || player.IsAttacking) AsignAlternativeTarget(0);
        if(!enemy.IsMoving) AsignAlternativeTarget(1);

        //Debug Targets with Spheres
        targetDebug[0].transform.position = targetGroup.m_Targets[0].target.position;
        targetDebug[1].transform.position = targetGroup.m_Targets[1].target.position;

        targetDebug[0].SetActive(debug);
        targetDebug[1].SetActive(debug);
    }

    #endregion
}
