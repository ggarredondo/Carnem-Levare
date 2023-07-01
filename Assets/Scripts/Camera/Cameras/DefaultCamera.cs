using UnityEngine;
using Cinemachine;
using System.Threading.Tasks;

public class DefaultCamera : MonoBehaviour, ICameraInitialize
{
    [Header("Orbital Movement")]
    [Range(0, 20)] [SerializeField] private float orbitalValue;
    [Range(0, 10)] [SerializeField] private float orbitalRecovery;
    [SerializeField] private float orbitalResponseTime;

    private Player player;
    private Enemy enemy;

    private Transform[] alternativeTargets;
    private Vector3[] defaultTargets;
    private bool hurt, isDoingMove, isBlocking;
    private float targetingSpeed, blockingCounter;

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

        enemy.StateMachine.HurtState.OnEnterHitbox += (in Hitbox hitbox) => CameraShake(hitbox);
        enemy.StateMachine.BlockedState.OnEnterHitbox += (in Hitbox hitbox) => BlockingCameraShake(hitbox);
        player.StateMachine.HurtState.OnEnterHitbox += (in Hitbox hitbox) => CameraShake(hitbox);
        player.StateMachine.BlockedState.OnEnterHitbox += (in Hitbox hitbox) => BlockingCameraShake(hitbox);

        enemy.StateMachine.HurtState.OnExit += () => hurt = false;
        enemy.StateMachine.BlockedState.OnExit += () => hurt = false;
        player.StateMachine.HurtState.OnExit += () => hurt = false;
        player.StateMachine.BlockedState.OnExit += () => hurt = false;

        player.StateMachine.WalkingState.OnEnter += () => isBlocking = false;
        player.StateMachine.BlockingState.OnEnter += () => isBlocking = true;
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
        if (player.Controller.MovementVector != new Vector2(0, 0) && !isBlocking && !isDoingMove)
            orbitalTransposer.m_XAxis.Value += player.Controller.MovementVector.x * orbitalValue * Time.deltaTime;

        if (isBlocking)
        {
            blockingCounter += Time.deltaTime * 1000;

            if (blockingCounter >= orbitalResponseTime)
                orbitalTransposer.m_XAxis.Value = Mathf.Lerp(orbitalTransposer.m_XAxis.Value, 0.5f, orbitalRecovery * Time.deltaTime);
        }
        else
            blockingCounter = 0;

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

    private void CameraShake(in Hitbox hitbox)
    {
        hurt = true;
        targetingSpeed = hitbox.HurtCameraShake.targetShakeIntensity;
        HurtTime(hitbox.HurtCameraShake.targetShakeTime);
    }

    private void BlockingCameraShake(in Hitbox hitbox)
    {
        hurt = true;
        targetingSpeed = hitbox.BlockedCameraShake.targetShakeIntensity;
        HurtTime(hitbox.BlockedCameraShake.targetShakeTime);
    }

    private async void HurtTime(double time)
    {
        await Task.Delay(System.TimeSpan.FromMilliseconds(time));
        hurt = false;
    }
}
