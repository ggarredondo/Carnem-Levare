using Cinemachine;
using UnityEngine;
using LerpUtilities;
using System.Threading.Tasks;
using System.Threading;

public class Noise : CameraEffect
{
    [Header("Parameters")]
    public Tuple<float> length;
    public Tuple<float> frequency;
    public Tuple<float> amplitude;

    private CinemachineBasicMultiChannelPerlin noiseTransposer;
    private CancellationTokenSource cancellationTokenSource;

    public override void Initialize(ref CinemachineVirtualCamera vcam)
    {
        this.vcam = vcam;
        noiseTransposer = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public override void UpdateCondition(ref Player player, ref Enemy enemy)
    {
        player.StateMachine.WalkingState.OnEnter += Positive;
        player.StateMachine.BlockingState.OnEnter += Negative;

        player.StateMachine.WalkingState.OnExit += Cancel;
        player.StateMachine.BlockingState.OnExit += Cancel;
    }

    private void Cancel()
    {
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
    }

    private async void Positive()
    {
        cancellationTokenSource = new();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        try
        {
            await Task.WhenAll(Lerp.Value_Cancel(noiseTransposer.m_FrequencyGain, frequency.Item1, f => noiseTransposer.m_FrequencyGain = f, length.Item1, cancellationToken),
                               Lerp.Value_Cancel(noiseTransposer.m_AmplitudeGain, amplitude.Item1, f => noiseTransposer.m_AmplitudeGain = f, length.Item1, cancellationToken));
        }
        catch (TaskCanceledException) { }
    }
    private async void Negative()
    {
        cancellationTokenSource = new();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        try
        {
            await Task.WhenAll(Lerp.Value_Cancel(noiseTransposer.m_FrequencyGain, frequency.Item2, f => noiseTransposer.m_FrequencyGain = f, length.Item2, cancellationToken),
                               Lerp.Value_Cancel(noiseTransposer.m_AmplitudeGain, amplitude.Item2, f => noiseTransposer.m_AmplitudeGain = f, length.Item2, cancellationToken));
        }
        catch (TaskCanceledException) { }
    }
}
