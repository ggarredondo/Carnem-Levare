using Cinemachine;

public class SmoothFollow : CameraMovement
{
    public Tuple<float> variation;

    private float reduce;
    private CinemachineOrbitalTransposer transposer;
    public enum Parameter { DAMPING, ORBITAL }
    public Parameter parameter;

    public override void Initialize()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        transposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    public override void ApplyMove(bool condition)
    {
        if(parameter == Parameter.DAMPING)
            transposer.m_YawDamping = CameraUtilities.OscillateParameter(condition, aceleration, ref reduce, variation, CameraUtilities.Exponential);
        else
            transposer.m_XAxis.Value = CameraUtilities.OscillateParameter(condition, aceleration, ref reduce, variation, CameraUtilities.Exponential);
    }
}
