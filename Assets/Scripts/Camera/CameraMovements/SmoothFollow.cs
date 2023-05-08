using Cinemachine;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/CameraEffects/SmoothFollow")]
public class SmoothFollow : CameraMovement
{
    public Tuple<float> variation;

    private float reduce;
    private CinemachineOrbitalTransposer transposer;

    public override void Initialize(ref CinemachineVirtualCamera vcam)
    {
        this.vcam = vcam;
        transposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    public override void UpdateCondition(ref Player player)
    {
        player.StateMachine.WalkingState.OnEnter += () => applyCondition = true;
        player.StateMachine.WalkingState.OnExit += () => applyCondition = false;
    }

    public override void ApplyMove()
    {
        transposer.m_YawDamping = CameraUtilities.OscillateParameter(applyCondition, aceleration, ref reduce, variation, CameraUtilities.Exponential);
    }
}
