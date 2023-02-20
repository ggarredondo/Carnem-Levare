using Cinemachine;
using UnityEngine;

public class DollyZoom : CameraMovement
{
    public float fieldOfViewVariation;
    public Vector3 offsetVariation;

    private float reduceFOV, reduceZoom, reduceNear;
    [HideInInspector] public Tuple<Vector3> zoomPositions;
    [HideInInspector] public Tuple<float> fieldOfView;
    [HideInInspector] public Tuple<float> nearPlane;
    private CinemachineTransposer transposer;

    private void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        transposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        nearPlane.Item1 = vcam.m_Lens.NearClipPlane;
        nearPlane.Item2 = 1;

        zoomPositions.Item1 = transposer.m_FollowOffset;
        zoomPositions.Item2 = zoomPositions.Item1 + offsetVariation;

        fieldOfView.Item1 = vcam.m_Lens.FieldOfView;
        fieldOfView.Item2 = fieldOfView.Item1 + fieldOfViewVariation;
    }

    public override void ApplyMove(bool condition)
    {
        transposer.m_FollowOffset = CameraUtilities.Movement(condition, aceleration, ref reduceZoom, new Vector3[] { zoomPositions.Item1, zoomPositions.Item2 }, CameraUtilities.LinearBezierCurve);
        vcam.m_Lens.FieldOfView = CameraUtilities.OscillateParameter(condition, aceleration, ref reduceFOV, fieldOfView, Mathf.Sin);
        vcam.m_Lens.NearClipPlane = CameraUtilities.OscillateParameter(condition, aceleration, ref reduceNear, nearPlane, Mathf.Sin);
    }
}
