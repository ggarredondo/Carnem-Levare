using Cinemachine;
using System.Threading;
using System.Threading.Tasks;

public class CameraShake : CameraEffect
{
    private CinemachineBasicMultiChannelPerlin noiseTransposer;

    public float defaultFrequency;
    public float defaultAmplitude;
    public NoiseSettings defaultNoiseSettings;

    private CancellationTokenSource cancellationTokenSource;

    public override void Initialize(ref CinemachineVirtualCamera vcam)
    {
        this.vcam = vcam;
        noiseTransposer = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        noiseTransposer.m_FrequencyGain = defaultFrequency;
        noiseTransposer.m_AmplitudeGain = defaultAmplitude;
        noiseTransposer.m_NoiseProfile = defaultNoiseSettings;

        cancellationTokenSource = new();
    }

    public override void UpdateCondition(ref Player player, ref Enemy enemy)
    {
        player.StateMachine.HurtState.OnEnterHitbox += (in Hitbox hitbox) => Shake(hitbox);
        player.StateMachine.BlockedState.OnEnterHitbox += (in Hitbox hitbox) => BlockingShake(hitbox);
        player.StateMachine.StaggerState.OnEnterHitbox += (in Hitbox hitbox) => StaggerShake(hitbox);
        player.StateMachine.KOState.OnEnterHitbox += (in Hitbox hitbox) => KOShake(hitbox);

        player.StateMachine.HurtState.OnExit += Cancel;
        player.StateMachine.BlockedState.OnExit += Cancel;
        player.StateMachine.StaggerState.OnExit += Cancel;
        player.StateMachine.KOState.OnExit += Cancel;
        player.StateMachine.MoveState.OnExit += Cancel;

        enemy.StateMachine.HurtState.OnEnterHitbox += (in Hitbox hitbox) => Shake(hitbox);
        enemy.StateMachine.BlockedState.OnEnterHitbox += (in Hitbox hitbox) => BlockingShake(hitbox);
        enemy.StateMachine.StaggerState.OnEnterHitbox += (in Hitbox hitbox) => StaggerShake(hitbox);
        enemy.StateMachine.KOState.OnEnterHitbox += (in Hitbox hitbox) => KOShake(hitbox);
    }

    private void Cancel()
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel();
        }
    }

    private void Shake(in Hitbox hitbox)
    {
        noiseTransposer.m_NoiseProfile = hitbox.HurtCameraShake.shakeType;
        noiseTransposer.m_FrequencyGain = hitbox.HurtCameraShake.screenShakeFrequency;
        noiseTransposer.m_AmplitudeGain = hitbox.HurtCameraShake.screenShakeAmplitude;

        Hurt((float) hitbox.HurtCameraShake.screenShakeTime);
    }

    private void BlockingShake(in Hitbox hitbox)
    {
        noiseTransposer.m_NoiseProfile = hitbox.BlockedCameraShake.shakeType;
        noiseTransposer.m_FrequencyGain = hitbox.BlockedCameraShake.screenShakeFrequency;
        noiseTransposer.m_AmplitudeGain = hitbox.BlockedCameraShake.screenShakeAmplitude;

        Hurt((float)hitbox.BlockedCameraShake.screenShakeTime);
    }

    private void StaggerShake(in Hitbox hitbox)
    {
        noiseTransposer.m_NoiseProfile = hitbox.StaggerCameraShake.shakeType;
        noiseTransposer.m_FrequencyGain = hitbox.StaggerCameraShake.screenShakeFrequency;
        noiseTransposer.m_AmplitudeGain = hitbox.StaggerCameraShake.screenShakeAmplitude;

        Hurt((float)hitbox.StaggerCameraShake.screenShakeTime);
    }

    private void KOShake(in Hitbox hitbox)
    {
        noiseTransposer.m_NoiseProfile = hitbox.KOCameraShake.shakeType;
        noiseTransposer.m_FrequencyGain = hitbox.KOCameraShake.screenShakeFrequency;
        noiseTransposer.m_AmplitudeGain = hitbox.KOCameraShake.screenShakeAmplitude;

        Hurt((float)hitbox.KOCameraShake.screenShakeTime);
    }

    private void ResetCancellationTokenSource()
    {
        cancellationTokenSource.Dispose();
        cancellationTokenSource = new();
    }

    private async void Hurt(float time)
    {
        await HurtTime_Async(time);

        noiseTransposer.m_FrequencyGain = defaultFrequency;
        noiseTransposer.m_AmplitudeGain = defaultAmplitude;
        noiseTransposer.m_NoiseProfile = defaultNoiseSettings;
    }

    private async Task HurtTime_Async(float time)
    {
        ResetCancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        try
        {
            await Task.Delay(System.TimeSpan.FromMilliseconds(time), cancellationToken);
        }
        catch (TaskCanceledException) {}
    }
}
