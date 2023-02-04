using Cinemachine;
using UnityEngine;

public class LinealMovement : CameraMovement
{
    public Tuple<float> fieldOfView;
    public Vector3 offsetVariation;

    private float reduce;
    private Tuple<Vector3> positions;
    private CinemachineTransposer transposer;

    public LinealMovement(CinemachineVirtualCamera vcam) : base(vcam) { transposer = vcam.GetCinemachineComponent<CinemachineTransposer>(); }

    private void Start()
    {
        positions.Item1 = transposer.m_FollowOffset;
        positions.Item2 = positions.Item1 + offsetVariation;
    }

    public override void ApplyMove(bool condition)
    {
        transposer.m_FollowOffset = CameraUtilities.Movement(condition, aceleration, ref reduce, new Vector3[] { positions.Item1, positions.Item2 }, CameraUtilities.LinearBezierCurve);
    }
}
