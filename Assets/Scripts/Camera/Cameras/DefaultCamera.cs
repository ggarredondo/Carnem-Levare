using UnityEngine;
using Cinemachine;

public class DefaultCamera : MonoBehaviour, ITargeting
{
    [Header("Target Group Parameters")]
    [Range(0, 40)] [SerializeField] private float targetingSpeed;

    [Header("Orbital Movement")]
    [Range(0, 20)] [SerializeField] private float orbitalValue;
    [Range(0, 10)] [SerializeField] private float orbitalRecovery;

    private Player player;
    private Enemy enemy;
    private Transform[] alternativeTargets;
    private bool changePlayerTargets, changeEnemyTargets, isBlocking, isMoving;

    private CinemachineOrbitalTransposer orbitalTransposer;
    private CinemachineTargetGroup targetGroup;

    public void Initialize(ref CinemachineTargetGroup targetGroup, ref CinemachineVirtualCamera vcam, ref Player player, ref Enemy enemy)
    {
        this.player = player;
        this.enemy = enemy;
        orbitalTransposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        this.targetGroup = targetGroup;

        alternativeTargets = new Transform[2];
        UpdateCameraConditions();
    }

    public void InitializeTarget(ref CameraTargets playerTargets, ref CameraTargets enemyTargets)
    {
        alternativeTargets[0] = playerTargets.GetAlternativeTarget(CameraType.DEFAULT);
        alternativeTargets[1] = enemyTargets.GetAlternativeTarget(CameraType.DEFAULT);
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
            OrbitalMovement();
            TargetUpdate();
        }
    }

    private void OrbitalMovement()
    {
        if (player.Controller.MovementVector != new Vector2(0, 0) && !isBlocking && !isMoving)
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
        if (changeEnemyTargets) AsignAlternativeTarget(1);
    }
}
