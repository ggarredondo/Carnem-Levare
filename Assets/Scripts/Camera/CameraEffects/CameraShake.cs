using Cinemachine;
using UnityEngine;
using LerpUtilities;
using System.Threading.Tasks;

[CreateAssetMenu(menuName = "Scriptable Objects/CameraEffects/CameraShake")]
public class CameraShake : CameraEffect
{
    private CinemachineBasicMultiChannelPerlin noiseTransposer;

    [SerializeField] private float startDuration;
    [SerializeField] private float finishDuration;

    private float defaultFrequency, defaultAmplitude;
    private NoiseSettings defaultNoiseSettings;

    public override void Initialize(ref CinemachineVirtualCamera vcam)
    {
        this.vcam = vcam;
        noiseTransposer = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public override void UpdateCondition(ref Player player, ref Enemy enemy)
    {
        player.StateMachine.HurtState.OnEnterHitbox += (in Hitbox hitbox) => Shake(hitbox);
        player.StateMachine.BlockedState.OnEnterHitbox += (in Hitbox hitbox) => BlockingShake(hitbox);
        player.StateMachine.StaggerState.OnEnterHitbox += (in Hitbox hitbox) => StaggerShake(hitbox);
        player.StateMachine.KOState.OnEnterHitbox += (in Hitbox hitbox) => KOShake(hitbox);

        enemy.StateMachine.HurtState.OnEnterHitbox += (in Hitbox hitbox) => Shake(hitbox);
        enemy.StateMachine.BlockedState.OnEnterHitbox += (in Hitbox hitbox) => BlockingShake(hitbox);
        enemy.StateMachine.StaggerState.OnEnterHitbox += (in Hitbox hitbox) => StaggerShake(hitbox);
        enemy.StateMachine.KOState.OnEnterHitbox += (in Hitbox hitbox) => KOShake(hitbox);
    }

    private void Shake(in Hitbox hitbox)
    {
        defaultFrequency = noiseTransposer.m_FrequencyGain;
        defaultAmplitude = noiseTransposer.m_AmplitudeGain;
        defaultNoiseSettings = noiseTransposer.m_NoiseProfile;

        noiseTransposer.m_NoiseProfile = hitbox.HurtCameraShake.shakeType;
        HurtTime(hitbox.HurtCameraShake.screenShakeFrequency, hitbox.HurtCameraShake.screenShakeAmplitude, (float) hitbox.HurtCameraShake.screenShakeTime);
    }

    private void BlockingShake(in Hitbox hitbox)
    {
        defaultFrequency = noiseTransposer.m_FrequencyGain;
        defaultAmplitude = noiseTransposer.m_AmplitudeGain;
        defaultNoiseSettings = noiseTransposer.m_NoiseProfile;

        noiseTransposer.m_NoiseProfile = hitbox.BlockedCameraShake.shakeType;
        HurtTime(hitbox.BlockedCameraShake.screenShakeFrequency, hitbox.BlockedCameraShake.screenShakeAmplitude, (float)hitbox.BlockedCameraShake.screenShakeTime);
    }

    private void StaggerShake(in Hitbox hitbox)
    {
        defaultFrequency = noiseTransposer.m_FrequencyGain;
        defaultAmplitude = noiseTransposer.m_AmplitudeGain;
        defaultNoiseSettings = noiseTransposer.m_NoiseProfile;

        noiseTransposer.m_NoiseProfile = hitbox.StaggerCameraShake.shakeType;
        HurtTime(hitbox.StaggerCameraShake.screenShakeFrequency, hitbox.StaggerCameraShake.screenShakeAmplitude, (float)hitbox.StaggerCameraShake.screenShakeTime);
    }

    private void KOShake(in Hitbox hitbox)
    {
        defaultFrequency = noiseTransposer.m_FrequencyGain;
        defaultAmplitude = noiseTransposer.m_AmplitudeGain;
        defaultNoiseSettings = noiseTransposer.m_NoiseProfile;

        noiseTransposer.m_NoiseProfile = hitbox.KOCameraShake.shakeType;
        HurtTime(hitbox.KOCameraShake.screenShakeFrequency, hitbox.KOCameraShake.screenShakeAmplitude, (float)hitbox.KOCameraShake.screenShakeTime);
    }

    private async void HurtTime(float frequency, float amplitude, float time)
    {
        await Task.WhenAll(Lerp.Value(noiseTransposer.m_FrequencyGain, frequency, f => noiseTransposer.m_FrequencyGain = f, startDuration),
                           Lerp.Value(noiseTransposer.m_AmplitudeGain, amplitude, f => noiseTransposer.m_AmplitudeGain = f, startDuration));

        await Task.Delay(System.TimeSpan.FromMilliseconds(time));

        await Task.WhenAll(Lerp.Value(noiseTransposer.m_FrequencyGain, defaultFrequency, f => noiseTransposer.m_FrequencyGain = f, finishDuration),
                           Lerp.Value(noiseTransposer.m_AmplitudeGain, defaultAmplitude, f => noiseTransposer.m_AmplitudeGain = f, finishDuration));

        noiseTransposer.m_NoiseProfile = defaultNoiseSettings;
    }
}
