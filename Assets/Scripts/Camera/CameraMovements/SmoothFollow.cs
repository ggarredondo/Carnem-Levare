using Cinemachine;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/CameraEffects/SmoothFollow")]
public class SmoothFollow : CameraMovement
{
    public Tuple<float> variation;

    private float reduce;
    private CinemachineOrbitalTransposer transposer;
    public enum Parameter { DAMPING, ORBITAL }
    public Parameter parameter;

    public override void Initialize(ref CinemachineVirtualCamera vcam)
    {
        this.vcam = vcam;
        transposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    public override void ApplyMove()
    {
        if(parameter == Parameter.DAMPING)
            transposer.m_YawDamping = CameraUtilities.OscillateParameter(applyCondition, aceleration, ref reduce, variation, CameraUtilities.Exponential);
        else
            transposer.m_XAxis.Value = CameraUtilities.OscillateParameter(applyCondition, aceleration, ref reduce, variation, CameraUtilities.Exponential);
    }
}
