using Cinemachine;
using UnityEngine;
using System.Collections;

public class CameraShake : CameraEffect
{
    private CinemachineBasicMultiChannelPerlin noiseTransposer;

    public float defaultFrequency;
    public float defaultAmplitude;
    public NoiseSettings defaultNoiseSettings;

    public override void Initialize(ref CinemachineVirtualCamera vcam)
    {
        this.vcam = vcam;
        noiseTransposer = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        noiseTransposer.m_FrequencyGain = defaultFrequency;
        noiseTransposer.m_AmplitudeGain = defaultAmplitude;
        noiseTransposer.m_NoiseProfile = defaultNoiseSettings;
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

        StopCoroutine(nameof(HurtTime));

        noiseTransposer.m_NoiseProfile = hitbox.HurtCameraShake.shakeType;
        noiseTransposer.m_FrequencyGain = hitbox.HurtCameraShake.screenShakeFrequency;
        noiseTransposer.m_AmplitudeGain = hitbox.HurtCameraShake.screenShakeAmplitude;

        StartCoroutine(HurtTime((float) hitbox.HurtCameraShake.screenShakeTime));
    }

    private void BlockingShake(in Hitbox hitbox)
    {

        StopCoroutine(nameof(HurtTime));

        noiseTransposer.m_NoiseProfile = hitbox.BlockedCameraShake.shakeType;
        noiseTransposer.m_FrequencyGain = hitbox.BlockedCameraShake.screenShakeFrequency;
        noiseTransposer.m_AmplitudeGain = hitbox.BlockedCameraShake.screenShakeAmplitude;

        StartCoroutine(HurtTime((float)hitbox.BlockedCameraShake.screenShakeTime));
    }

    private void StaggerShake(in Hitbox hitbox)
    {

        StopCoroutine(nameof(HurtTime));

        noiseTransposer.m_NoiseProfile = hitbox.StaggerCameraShake.shakeType;
        noiseTransposer.m_FrequencyGain = hitbox.StaggerCameraShake.screenShakeFrequency;
        noiseTransposer.m_AmplitudeGain = hitbox.StaggerCameraShake.screenShakeAmplitude;

        StartCoroutine(HurtTime((float)hitbox.StaggerCameraShake.screenShakeTime));
    }

    private void KOShake(in Hitbox hitbox)
    {
        StopCoroutine(nameof(HurtTime));

        noiseTransposer.m_NoiseProfile = hitbox.KOCameraShake.shakeType;
        noiseTransposer.m_FrequencyGain = hitbox.KOCameraShake.screenShakeFrequency;
        noiseTransposer.m_AmplitudeGain = hitbox.KOCameraShake.screenShakeAmplitude;

        StartCoroutine(HurtTime((float)hitbox.KOCameraShake.screenShakeTime));
    }

    private IEnumerator HurtTime(float time)
    {
        yield return new WaitForSeconds((float) System.TimeSpan.FromMilliseconds(time).TotalSeconds);

        noiseTransposer.m_FrequencyGain = defaultFrequency;
        noiseTransposer.m_AmplitudeGain = defaultAmplitude;
        noiseTransposer.m_NoiseProfile = defaultNoiseSettings;
    }
}
