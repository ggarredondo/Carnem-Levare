using Cinemachine;
using LerpUtilities;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/CameraEffects/LinealMovement")]
public class LinealMovement : CameraMovement
{
    public Vector3 offsetVariation;

    private Tuple<Vector3> positions;
    private CinemachineTransposer transposer;

    //SaveInitialPosition
    private Vector3 initialPosition;

    public override void Initialize(ref CinemachineVirtualCamera vcam)
    {
        this.vcam = vcam;
        transposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        UpdateParameters();

        initialPosition = positions.Item1;
    }

    public override void UpdateCondition(ref Player player, ref Enemy enemy)
    {
        Player playerLocal = player;
        player.Controller.OnDoBlock += () => 
        { 
            applyCondition = playerLocal.Controller.isBlocking;
            Movement();
        };
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

    private async void Movement()
    {
        if(applyCondition)
            await Lerp.Value_Bezier(new Vector3[] { positions.Item1, positions.Item2 }, v => transposer.m_FollowOffset = v, duration.Item1, CameraUtilities.LinearBezierCurve);
        else
            await Lerp.Value_Bezier(new Vector3[] { positions.Item2, positions.Item1 }, v => transposer.m_FollowOffset = v, duration.Item2, CameraUtilities.LinearBezierCurve);
    }
}
