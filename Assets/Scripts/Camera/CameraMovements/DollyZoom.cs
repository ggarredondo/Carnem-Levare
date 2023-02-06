using Cinemachine;
using UnityEngine;

public class DollyZoom : CameraMovement
{
    public Tuple<float> fieldOfView;
    public Vector3 offsetVariation;

    private float reduceFOV, reduceZoom;
    public Tuple<Vector3> zoomPositions;
    private CinemachineTransposer transposer;

    public override void Awake()
    {
        base.Awake();
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        zoomPositions.Item1 = transposer.m_FollowOffset;
        zoomPositions.Item2 = zoomPositions.Item1 + offsetVariation;
    }

    public override void ApplyMove(bool condition)
    {
        transposer.m_FollowOffset = CameraUtilities.Movement(condition, aceleration, ref reduceZoom, new Vector3[] { zoomPositions.Item1, zoomPositions.Item2 }, CameraUtilities.LinearBezierCurve);
        vcam.m_Lens.FieldOfView = CameraUtilities.OscillateParameter(condition, aceleration, ref reduceFOV, fieldOfView, Mathf.Sin);
    }
}
