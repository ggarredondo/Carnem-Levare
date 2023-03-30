using Cinemachine;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/CameraEffects/LinealMovement")]
public class LinealMovement : CameraMovement
{
    public Vector3 offsetVariation;

    private float reduce;
    private Tuple<Vector3> positions;
    private CinemachineTransposer transposer;

    //SaveInitialPosition
    private Vector3 initialPosition;

    public override void Initialize(CinemachineVirtualCamera vcam)
    {
        this.vcam = vcam;
        transposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        UpdateParameters();

        initialPosition = positions.Item1;
    }

    protected override void UpdateParameters()
    {
        positions.Item1 = transposer.m_FollowOffset;
        positions.Item2 = positions.Item1 + offsetVariation;
    }

    public override void UpdateInitialPosition()
    {
        positions.Item1 = transposer.m_FollowOffset;
    }

    public override void ReturnInitialPosition()
    {
        positions.Item1 = initialPosition;
    }

    public override void ApplyMove(bool condition)
    {
        transposer.m_FollowOffset = CameraUtilities.Movement(condition, aceleration, ref reduce, new Vector3[] { positions.Item1, positions.Item2 }, CameraUtilities.LinearBezierCurve);
    }
}
