using UnityEngine;
using Cinemachine;
using System.Threading.Tasks;

public class DefaultCamera : MonoBehaviour, ICameraInitialize
{
    [Header("Target Group Parameters")]
    [Range(0, 100)] [SerializeField] private float targetingSpeed;
    [SerializeField] private float hurtTime;

    [Header("Orbital Movement")]
    [Range(0, 20)] [SerializeField] private float orbitalValue;
    [Range(0, 10)] [SerializeField] private float orbitalRecovery;

    private Player player;
    private Enemy enemy;
    private Transform[] alternativeTargets;
    private bool hurtEnemy, isBlocking, isDoingMove;

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
        player.StateMachine.MoveState.OnEnter += () => isDoingMove = true;
        player.StateMachine.MoveState.OnExit += () => isDoingMove = false;

        enemy.StateMachine.HurtState.OnEnter += () => { hurtEnemy = true; HurtTime(); };
        enemy.StateMachine.HurtState.OnExit += () => hurtEnemy = false;
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
        if (player.Controller.MovementVector != new Vector2(0, 0) && !player.Controller.isBlocking && !isDoingMove)
            orbitalTransposer.m_XAxis.Value += player.Controller.MovementVector.x * orbitalValue * Time.deltaTime;

        if (!player.Controller.isBlocking)
            orbitalTransposer.m_XAxis.Value = Mathf.Lerp(orbitalTransposer.m_XAxis.Value, 0, orbitalRecovery * Time.deltaTime);

        orbitalTransposer.m_RecenterToTargetHeading.m_enabled = player.Controller.MovementVector != new Vector2(0, 0) && !!player.Controller.isBlocking && !isDoingMove;
    }

    private void AsignAlternativeTarget(int index)
    {
        targetGroup.m_Targets[index].target.position = Vector3.Lerp(targetGroup.m_Targets[index].target.position, alternativeTargets[index].position, targetingSpeed * Time.deltaTime);
    }

    private void TargetUpdate()
    {
        if (hurtEnemy)
        {
            AsignAlternativeTarget(0);
            AsignAlternativeTarget(1);
        }
    }

    private async void HurtTime()
    {
        await Task.Delay(System.TimeSpan.FromMilliseconds(hurtTime));
        hurtEnemy = false;
    }
}
