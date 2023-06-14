using Cinemachine;
using UnityEngine;
using LerpUtilities;
using System.Threading.Tasks;

[CreateAssetMenu(menuName = "Scriptable Objects/CameraEffects/Noise")]
public class Noise : CameraMovement
{
    public Tuple<float> frequency;
    public Tuple<float> amplitude;

    private CinemachineBasicMultiChannelPerlin noiseTransposer;

    public override void Initialize(ref CinemachineVirtualCamera vcam)
    {
        this.vcam = vcam;
        noiseTransposer = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public override void UpdateCondition(ref Player player, ref Enemy enemy)
    {
        player.StateMachine.WalkingState.OnEnter += Positive;
        player.StateMachine.BlockingState.OnEnter += Negative;
    }

    private async void Positive()
    {
        await Task.WhenAll(Lerp.Value(noiseTransposer.m_FrequencyGain, frequency.Item1, f => noiseTransposer.m_FrequencyGain = f, duration.Item1),
                           Lerp.Value(noiseTransposer.m_AmplitudeGain, amplitude.Item1, f => noiseTransposer.m_AmplitudeGain = f, duration.Item1));
    }
    private async void Negative()
    {
        await Task.WhenAll(Lerp.Value(noiseTransposer.m_FrequencyGain, frequency.Item2, f => noiseTransposer.m_FrequencyGain = f, duration.Item2),
                           Lerp.Value(noiseTransposer.m_AmplitudeGain, amplitude.Item2, f => noiseTransposer.m_AmplitudeGain = f, duration.Item2));
    }
}
