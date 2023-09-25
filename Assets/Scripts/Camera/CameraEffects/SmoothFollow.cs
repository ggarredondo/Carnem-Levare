using Cinemachine;
using LerpUtilities;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/CameraEffects/SmoothFollow")]
public class SmoothFollow : CameraEffect
{
    public Tuple<float> variation;

    private CinemachineOrbitalTransposer transposer;

    public override void Initialize(ref CinemachineVirtualCamera vcam)
    {
        this.vcam = vcam;
        transposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    public override void UpdateCondition(ref Player player, ref Enemy enemy)
    {
        player.StateMachine.WalkingState.OnEnter += Negative;
        player.StateMachine.WalkingState.OnExit += Positive;
    }

    private async void Positive()
    {
        await Lerp.Value_Math(transposer.m_YawDamping, variation.Item1, f => transposer.m_YawDamping = f, speed.Item1, CameraUtilities.Exponential);
    }
    private async void Negative()
    {
        await Lerp.Value_Math(transposer.m_YawDamping, variation.Item2, f => transposer.m_YawDamping = f, speed.Item2, CameraUtilities.Exponential);
    }
}
