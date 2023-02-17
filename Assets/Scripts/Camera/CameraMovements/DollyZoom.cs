using Cinemachine;
using UnityEngine;

public class DollyZoom : CameraMovement
{
    public float fieldOfViewVariation;
    public Vector3 offsetVariation;

    private float reduceFOV, reduceZoom;
    [HideInInspector] public Tuple<Vector3> zoomPositions;
    [HideInInspector] public Tuple<float> fieldOfView;
    private CinemachineTransposer transposer;

    public override void Awake()
    {
        base.Awake();
        transposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        zoomPositions.Item1 = transposer.m_FollowOffset;
        zoomPositions.Item2 = zoomPositions.Item1 + offsetVariation;

        fieldOfView.Item1 = vcam.m_Lens.FieldOfView;
        fieldOfView.Item2 = fieldOfView.Item1 + fieldOfViewVariation;
    }

    public override void ApplyMove(bool condition)
    {
        transposer.m_FollowOffset = CameraUtilities.Movement(condition, aceleration, ref reduceZoom, new Vector3[] { zoomPositions.Item1, zoomPositions.Item2 }, CameraUtilities.LinearBezierCurve);
        vcam.m_Lens.FieldOfView = CameraUtilities.OscillateParameter(condition, aceleration, ref reduceFOV, fieldOfView, Mathf.Sin);
    }
}
