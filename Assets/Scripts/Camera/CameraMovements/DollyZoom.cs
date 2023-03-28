using Cinemachine;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/CameraEffects/DollyZoom")]
public class DollyZoom : CameraMovement
{
    public float fieldOfViewVariation;
    public Vector3 offsetVariation;

    [System.NonSerialized] public Tuple<Vector3> zoomPositions;
    [System.NonSerialized] public Tuple<float> fieldOfView;
    [System.NonSerialized] public Tuple<float> nearPlane;
    private CinemachineTransposer transposer;
    private float reduceFOV, reduceZoom, reduceNear;

    //SaveInitialPosition
    private Vector3 initialZoomPosition;
    private float initialFieldOfView;
    private float initialNearPlane;

    public override void Initialize(CinemachineVirtualCamera vcam)
    {
        this.vcam = vcam;
        transposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        UpdateParameters();

        initialZoomPosition = zoomPositions.Item1;
        initialFieldOfView = fieldOfView.Item1;
        initialNearPlane = nearPlane.Item1;
    }

    protected override void UpdateParameters()
    {
        nearPlane.Item1 = vcam.m_Lens.NearClipPlane;
        nearPlane.Item2 = 1;

        zoomPositions.Item1 = transposer.m_FollowOffset;
        zoomPositions.Item2 = zoomPositions.Item1 + offsetVariation;

        fieldOfView.Item1 = vcam.m_Lens.FieldOfView;
        fieldOfView.Item2 = fieldOfView.Item1 + fieldOfViewVariation;
    }

    public override void UpdateInitialPosition()
    {
        zoomPositions.Item1 = transposer.m_FollowOffset;
        fieldOfView.Item1 = vcam.m_Lens.FieldOfView;
        nearPlane.Item1 = vcam.m_Lens.NearClipPlane;
    }

    public override void ReturnInitialPosition()
    {
        zoomPositions.Item1 = initialZoomPosition;
        fieldOfView.Item1 = initialFieldOfView;
        nearPlane.Item1 = initialNearPlane;
    }

    public override void ApplyMove(bool condition)
    {
        transposer.m_FollowOffset = CameraUtilities.Movement(condition, aceleration, ref reduceZoom, new Vector3[] { zoomPositions.Item1, zoomPositions.Item2 }, CameraUtilities.LinearBezierCurve);
        vcam.m_Lens.FieldOfView = CameraUtilities.OscillateParameter(condition, aceleration, ref reduceFOV, fieldOfView, Mathf.Sin);
        vcam.m_Lens.NearClipPlane = CameraUtilities.OscillateParameter(condition, aceleration, ref reduceNear, nearPlane, Mathf.Sin);
    }
}
