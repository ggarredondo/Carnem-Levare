using Cinemachine;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/CameraEffects/LinealMovement")]
public class LinealMovement : CameraMovement
{
    public Vector3 offsetVariation;

    private float reduce;
    private Tuple<Vector3> positions;
    private CinemachineTransposer transposer;

    public override void Initialize(CinemachineVirtualCamera vcam)
    {
        this.vcam = vcam;
        transposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        UpdateParameters();
    }

    public override void UpdateParameters()
    {
        positions.Item1 = transposer.m_FollowOffset;
        positions.Item2 = positions.Item1 + offsetVariation;
    }

    public override bool InitialPosition()
    {
        return positions.Item1 == transposer.m_FollowOffset;
    }

    public override void ApplyMove(bool condition)
    {
        transposer.m_FollowOffset = CameraUtilities.Movement(condition, aceleration, ref reduce, new Vector3[] { positions.Item1, positions.Item2 }, CameraUtilities.LinearBezierCurve);
    }
}
