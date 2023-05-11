using UnityEngine;
using Cinemachine;
using System.Threading.Tasks;

public class DefaultCamera : MonoBehaviour, ICameraInitialize
{
    [Header("Orbital Movement")]
    [Range(0, 20)] [SerializeField] private float orbitalValue;
    [Range(0, 10)] [SerializeField] private float orbitalRecovery;

    private Player player;
    private Enemy enemy;

    private Transform[] alternativeTargets;
    private Vector3[] defaultTargets;
    private bool hurt, isDoingMove;
    private float targetingSpeed;

    private CinemachineOrbitalTransposer orbitalTransposer;
    private CinemachineTargetGroup targetGroup;

    public void Initialize(ref CinemachineTargetGroup targetGroup, ref CinemachineVirtualCamera vcam, ref Player player, ref Enemy enemy)
    {
        this.player = player;
        this.enemy = enemy;
        orbitalTransposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        this.targetGroup = targetGroup;

        alternativeTargets = new Transform[2];
        defaultTargets = new Vector3[2];

        UpdateCameraConditions();
    }

    public void InitializeTarget(ref CameraTargets playerTargets, ref CameraTargets enemyTargets)
    {
        alternativeTargets[0] = playerTargets.GetAlternativeTarget(CameraType.DEFAULT);
        alternativeTargets[1] = enemyTargets.GetAlternativeTarget(CameraType.DEFAULT);

        defaultTargets[0] = targetGroup.m_Targets[0].target.localPosition;
        defaultTargets[1] = targetGroup.m_Targets[1].target.localPosition;
    }

    private void UpdateCameraConditions()
    {
        player.StateMachine.MoveState.OnEnter += () => isDoingMove = true;
        player.StateMachine.MoveState.OnExit += () => isDoingMove = false;

        enemy.StateMachine.HurtState.OnEnter += () => 
        { 
            hurt = true;
            targetingSpeed = enemy.StateMachine.HurtState.Hitbox.HitShakeIntensity;
            HurtTime(enemy.StateMachine.HurtState.Hitbox.HitShakeTime); 
        };

        enemy.StateMachine.HurtState.OnExit += () => hurt = false;

        player.StateMachine.HurtState.OnEnter += () =>
        {
            hurt = true;
            targetingSpeed = player.StateMachine.HurtState.Hitbox.HitShakeIntensity;
            HurtTime(player.StateMachine.HurtState.Hitbox.HitShakeTime);
        };

        player.StateMachine.HurtState.OnExit += () => hurt = false;

        player.StateMachine.BlockedState.OnEnter += () =>
        {
            hurt = true;
            targetingSpeed = player.StateMachine.HurtState.Hitbox.HitShakeIntensity / 2;
            HurtTime(player.StateMachine.HurtState.Hitbox.HitShakeTime);
        };

        player.StateMachine.BlockedState.OnExit += () => hurt = false;

        enemy.StateMachine.BlockedState.OnEnter += () =>
        {
            hurt = true;
            targetingSpeed = enemy.StateMachine.HurtState.Hitbox.HitShakeIntensity / 2;
            HurtTime(enemy.StateMachine.HurtState.Hitbox.HitShakeTime);
        };

        enemy.StateMachine.BlockedState.OnExit += () => hurt = false;
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

        if (player.Controller.isBlocking)
            orbitalTransposer.m_XAxis.Value = Mathf.Lerp(orbitalTransposer.m_XAxis.Value, 0, orbitalRecovery * Time.deltaTime);

        orbitalTransposer.m_RecenterToTargetHeading.m_enabled = player.Controller.MovementVector != new Vector2(0, 0) && !player.Controller.isBlocking && !isDoingMove;
    }

    private void AsignAlternativeTarget()
    {
        targetGroup.m_Targets[0].target.position = Vector3.Lerp(targetGroup.m_Targets[0].target.position, alternativeTargets[0].position, targetingSpeed * Time.deltaTime);
        targetGroup.m_Targets[1].target.position = Vector3.Lerp(targetGroup.m_Targets[1].target.position, alternativeTargets[1].position, targetingSpeed * Time.deltaTime);
    }

    private void AsignDefaultTarget()
    {
        targetGroup.m_Targets[0].target.localPosition = Vector3.Lerp(targetGroup.m_Targets[0].target.localPosition, defaultTargets[0], targetingSpeed * Time.deltaTime);
        targetGroup.m_Targets[1].target.localPosition = Vector3.Lerp(targetGroup.m_Targets[1].target.localPosition, defaultTargets[1], targetingSpeed * Time.deltaTime);
    }

    private void TargetUpdate()
    {
        if (hurt)
        {
            AsignAlternativeTarget();
        }
        else
        {
            AsignDefaultTarget();
        }
    }

    private async void HurtTime(double time)
    {
        await Task.Delay(System.TimeSpan.FromMilliseconds(time));
        hurt = false;
    }
}
