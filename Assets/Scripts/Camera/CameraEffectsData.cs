using Cinemachine;
using UnityEngine;

[System.Serializable]
public class CameraEffectsData
{
    [Header("Target shake")]
    public double targetShakeTime;
    public float targetShakeIntensity;

    [Header("Screen shake")]
    public NoiseSettings shakeType;
    public double screenShakeTime;
    public float screenShakeFrequency;
    public float screenShakeAmplitude;
}