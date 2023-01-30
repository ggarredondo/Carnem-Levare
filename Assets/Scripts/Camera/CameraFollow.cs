using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{

    //Max value of the damping parameter (Virtual camera)
    const float MAX = 1;
    const float MIN = 0;
    const float CAMERA_SPEED_DIVIDER = 1000;

    [Header("Target Objects")]
    public PlayerController playerController;
    public CinemachineVirtualCamera vcam;

    [Header("Camera Parameters")]
    [Range(0f, 5f)] public float cameraAceleration;
    public float MAX_DAMPING = 20;
    public float MIN_DAMPING = 0;
    public bool cameraRotation;

    private float reduceDamping;
    private CinemachineTransposer transposer;

    [Header("Noise Parameters")]
    [Range(0f, 5f)] public float noiseAceleration;
    public float MAX_NOISE = 3;
    public float MIN_NOISE = 1;
    public bool cameraNoise;

    private float reduceNoise;
    private CinemachineBasicMultiChannelPerlin noiseTransposer;

    private void Awake()
    {
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
        noiseTransposer = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        reduceDamping = MIN;
    }

    private void LateUpdate()
    {
        if (Time.timeScale == 1)
        {
            if (cameraRotation)
                transposer.m_YawDamping = OscillateWithVelocity(playerController.getIsMoving, cameraAceleration, ref reduceDamping, MIN_DAMPING, MAX_DAMPING);

            if (cameraNoise)
                noiseTransposer.m_FrequencyGain = OscillateWithVelocity(playerController.getIsMovement, noiseAceleration, ref reduceNoise, MIN_NOISE, MAX_NOISE);
        }
    }

    private float NormalizeToInterval(float num, float min, float max)
    {
        return num * (max - min) + min;
    }

    private float Function(float x)
    {
        return Mathf.Pow(x, 4);
    }

    private float OscillateWithVelocity(bool condition, float aceleration, ref float reduce, float min, float max)
    {
        //Apply the reduce to the actual value
        float value = MAX * Function(reduce);

        //Increment or Decrement the reduce to make feel aceleration
        if (condition) reduce += aceleration / CAMERA_SPEED_DIVIDER;
        else           reduce -= aceleration / CAMERA_SPEED_DIVIDER;

        //Make values be under damping parameter range
        reduce = Mathf.Clamp(reduce, MIN, MAX);

        //Apply to the virtual camera parameter
        return NormalizeToInterval(value, min, max);   
    }
}