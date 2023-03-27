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

    public override void Initialize(CinemachineVirtualCamera vcam)
    {
        this.vcam = vcam;
        transposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        UpdateParameters();
    }

    public override void UpdateParameters()
    {
        nearPlane.Item1 = vcam.m_Lens.NearClipPlane;
        nearPlane.Item2 = 1;

        zoomPositions.Item1 = transposer.m_FollowOffset;
        zoomPositions.Item2 = zoomPositions.Item1 + offsetVariation;

        fieldOfView.Item1 = vcam.m_Lens.FieldOfView;
        fieldOfView.Item2 = fieldOfView.Item1 + fieldOfViewVariation;
    }

    public override bool InitialPosition()
    {
        return transposer.m_FollowOffset == zoomPositions.Item1 &&
               vcam.m_Lens.FieldOfView == fieldOfView.Item1 &&
               vcam.m_Lens.NearClipPlane == nearPlane.Item1;
    }

    public override void ApplyMove(bool condition)
    {
        transposer.m_FollowOffset = CameraUtilities.Movement(condition, aceleration, ref reduceZoom, new Vector3[] { zoomPositions.Item1, zoomPositions.Item2 }, CameraUtilities.LinearBezierCurve);
        vcam.m_Lens.FieldOfView = CameraUtilities.OscillateParameter(condition, aceleration, ref reduceFOV, fieldOfView, Mathf.Sin);
        vcam.m_Lens.NearClipPlane = CameraUtilities.OscillateParameter(condition, aceleration, ref reduceNear, nearPlane, Mathf.Sin);
    }
}
