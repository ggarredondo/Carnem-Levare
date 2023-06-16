using Cinemachine;
using LerpUtilities;
using System.Threading.Tasks;
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
        player.StateMachine.WalkingState.OnEnter += () => 
        {
            applyCondition = false;
            NegativeMovement();
        };

        player.StateMachine.BlockingState.OnEnter += () =>
        {
            applyCondition = true;
            PositiveMovement();
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

    private async void PositiveMovement()
    {
        float elapsedTime = 0f;

        while (elapsedTime < responseTime && applyCondition)
        {
            elapsedTime += Time.deltaTime * 1000;
            await Task.Yield();
        }

        if (!applyCondition) return;

        await Lerp.Value_Bezier(new Vector3[] { transposer.m_FollowOffset, positions.Item2 }, v => transposer.m_FollowOffset = v, speed.Item1, CameraUtilities.LinearBezierCurve);
    }

    private async void NegativeMovement()
    {
        float elapsedTime = 0f;

        while (elapsedTime < responseTime && !applyCondition)
        {
            elapsedTime += Time.deltaTime * 1000;
            await Task.Yield();
        }

        if (applyCondition) return;

        await Lerp.Value_Bezier(new Vector3[] { transposer.m_FollowOffset, positions.Item1 }, v => transposer.m_FollowOffset = v, speed.Item2, CameraUtilities.LinearBezierCurve);
    }
}
