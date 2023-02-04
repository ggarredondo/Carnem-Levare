using Cinemachine;

public class SmoothFollow : CameraMovement
{
    public Tuple<float> damping;

    private float reduce;
    private CinemachineTransposer transposer;

    public SmoothFollow(CinemachineVirtualCamera vcam) : base(vcam) { transposer = vcam.GetCinemachineComponent<CinemachineTransposer>(); }

    public override void ApplyMove(bool condition)
    {
        transposer.m_YawDamping = CameraUtilities.OscillateParameter(condition, aceleration, ref reduce, damping, CameraUtilities.Exponential);
    }
}
