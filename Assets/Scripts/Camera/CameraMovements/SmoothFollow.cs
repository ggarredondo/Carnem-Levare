using Cinemachine;

public class SmoothFollow : CameraMovement
{
    public Tuple<float> damping;

    private float reduce;
    private CinemachineTransposer transposer;

    public override void Awake()
    {
        base.Awake();
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
    }

    public override void ApplyMove(bool condition)
    {
        transposer.m_YawDamping = CameraUtilities.OscillateParameter(condition, aceleration, ref reduce, damping, CameraUtilities.Exponential);
    }
}
