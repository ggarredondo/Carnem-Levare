using Cinemachine;

public class Noise : CameraMovement
{
    public Tuple<float> frequency;
    public Tuple<float> amplitude;

    private float reduceAmplitude, reduceFrequency;
    private CinemachineBasicMultiChannelPerlin noiseTransposer;

    public override void Awake()
    {
        base.Awake();
        noiseTransposer = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Initialize()
    {
        vcam = GetComponentInChildren<CinemachineVirtualCamera>();
        noiseTransposer = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public override void ApplyMove(bool condition)
    {
        noiseTransposer.m_FrequencyGain = CameraUtilities.OscillateParameter(condition, aceleration, ref reduceFrequency, frequency, CameraUtilities.Exponential);
        noiseTransposer.m_AmplitudeGain = CameraUtilities.OscillateParameter(condition, aceleration, ref reduceAmplitude, amplitude, CameraUtilities.Exponential);
    }
}
