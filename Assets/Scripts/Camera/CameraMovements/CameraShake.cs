using Cinemachine;
using UnityEngine;
using LerpUtilities;
using System.Threading.Tasks;

[CreateAssetMenu(menuName = "Scriptable Objects/CameraEffects/CameraShake")]
public class CameraShake : CameraMovement
{
    private CinemachineBasicMultiChannelPerlin noiseTransposer;
    private Player player;
    private Enemy enemy;

    [SerializeField] private float startDuration;
    [SerializeField] private float finishDuration;

    private float defaultFrequency, defaultAmplitude;

    public override void Initialize(ref CinemachineVirtualCamera vcam)
    {
        this.vcam = vcam;
        noiseTransposer = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public override void UpdateCondition(ref Player player, ref Enemy enemy)
    {
        this.player = player;
        this.enemy = enemy;

        player.StateMachine.HurtState.OnEnter += () => Shake(this.player.StateMachine.HurtState.Hitbox);
        player.StateMachine.BlockedState.OnEnter += () => Shake(this.player.StateMachine.BlockedState.Hitbox);

        enemy.StateMachine.HurtState.OnEnter += () => Shake(this.enemy.StateMachine.HurtState.Hitbox);
        enemy.StateMachine.BlockedState.OnEnter += () => Shake(this.enemy.StateMachine.BlockedState.Hitbox);
    }

    private void Shake(in Hitbox hitbox)
    {
        defaultFrequency = noiseTransposer.m_FrequencyGain;
        defaultAmplitude = noiseTransposer.m_AmplitudeGain;

        HurtTime(hitbox.ScreenShakeFrequency, hitbox.ScreenShakeAmplitude, (float) hitbox.ScreenShakeTime);
    }

    private async void HurtTime(float frequency, float amplitude, float time)
    {
        await Task.WhenAll(Lerp.Value(noiseTransposer.m_FrequencyGain, frequency, f => noiseTransposer.m_FrequencyGain = f, startDuration),
                           Lerp.Value(noiseTransposer.m_AmplitudeGain, amplitude, f => noiseTransposer.m_AmplitudeGain = f, startDuration));

        await Task.Delay(System.TimeSpan.FromMilliseconds(time));

        await Task.WhenAll(Lerp.Value(noiseTransposer.m_FrequencyGain, defaultFrequency, f => noiseTransposer.m_FrequencyGain = f, finishDuration),
                           Lerp.Value(noiseTransposer.m_AmplitudeGain, defaultAmplitude, f => noiseTransposer.m_AmplitudeGain = f, finishDuration));
    }
}
