using UnityEngine;
using Cinemachine;
using System;

public class CameraFollow : MonoBehaviour
{

    //Max value of the damping parameter (Virtual camera)
    const float MAX = 1;
    const float MIN = 0;
    const float CAMERA_SPEED_DIVIDER = 1000;

    private float holdingMinTime;
    public int chargeLimitDivider;

    [Header("Target Objects")]
    public PlayerController playerController;
    public CinemachineVirtualCamera vcam;

    [Header("Follow Parameters")]
    [Range(0f, 5f)] public float cameraAceleration;
    public float MAX_DAMPING = 20;
    public float MIN_DAMPING = 0;
    public bool cameraRotation;

    private float reduceDamping;
    private CinemachineTransposer transposer;

    [Header("Noise Parameters")]
    [Range(0f, 5f)] public float noiseAceleration;
    public float MAX_FREQUENCY = 3;
    public float MIN_FREQUENCY = 1;
    public float MAX_AMPLITUDE = 3;
    public float MIN_AMPLITUDE = 1;
    public bool cameraNoise;

    private float reduceAmplitude, reduceFrequency;
    private CinemachineBasicMultiChannelPerlin noiseTransposer;

    [Header("Movement Parameters")]
    public float[] zoomAceleration;
    public Vector3[] blockingPositions;
    public float[] fieldOfView;
    public bool cameraMovement;

    private float reduceFOV, reduceBlock;

    [HideInInspector]public Move currentMove;

    private void Awake()
    {
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
        noiseTransposer = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        currentMove = playerController.rightNormalSlot;
    }

    public void Initialized()
    {
        holdingMinTime = currentMove.getChargeLimit / chargeLimitDivider;
        zoomAceleration[0] = 1 / (currentMove.getChargeLimit - holdingMinTime);
    }

    private void LateUpdate()
    {
        if (Time.timeScale > 0f)
        {
            if (cameraRotation)
            {
                transposer.m_YawDamping = OscillateWithVelocity(playerController.getIsMoving, cameraAceleration, ref reduceDamping, MIN_DAMPING, MAX_DAMPING, Exponential);
            }

            if (cameraMovement)
            {
                transposer.m_FollowOffset = LinealCameraMovement(currentMove.getChargePhase == Move.ChargePhase.performing && currentMove.getDeltaTimer >= holdingMinTime && currentMove.getDeltaTimer <= currentMove.getChargeLimit,
                                                                 zoomAceleration, ref reduceBlock, blockingPositions[0], blockingPositions[1]);

                vcam.m_Lens.FieldOfView = OscillateWithVelocity(currentMove.getChargePhase == Move.ChargePhase.performing && currentMove.getDeltaTimer >= holdingMinTime && currentMove.getDeltaTimer <= currentMove.getChargeLimit,
                                                                zoomAceleration, ref reduceFOV, fieldOfView[0], fieldOfView[1], Mathf.Sin);
            }

            if (cameraNoise)
            {
                noiseTransposer.m_FrequencyGain = OscillateWithVelocity(playerController.getIsMovement, noiseAceleration, ref reduceFrequency, MIN_FREQUENCY, MAX_FREQUENCY, Exponential);
                noiseTransposer.m_AmplitudeGain = OscillateWithVelocity(playerController.getIsMovement, noiseAceleration, ref reduceAmplitude, MIN_AMPLITUDE, MAX_AMPLITUDE, Exponential);
            }
        }
    }

    private float NormalizeToInterval(float num, float min, float max)
    {
        return num * (max - min) + min;
    }

    private float Exponential(float x)
    {
        return Mathf.Pow(x, 4);
    }

    private float OscillateWithVelocity(bool condition, float aceleration, ref float reduce, float min, float max, Func<float, float> function)
    {
        //Apply the reduce to the actual value
        float value = MAX * function(reduce);

        //Increment or Decrement the reduce to make feel aceleration
        if (condition) reduce += aceleration * Time.deltaTime;
        else           reduce -= aceleration * Time.deltaTime;

        //Make values be under damping parameter range
        reduce = Mathf.Clamp(reduce, MIN, MAX);

        //Apply to the virtual camera parameter
        return NormalizeToInterval(value, min, max);   
    }

    private float OscillateWithVelocity(bool condition, float[] aceleration, ref float reduce, float min, float max, Func<float, float> function)
    {
        //Apply the reduce to the actual value
        float value = MAX * function(reduce);

        //Increment or Decrement the reduce to make feel aceleration
        if (condition) reduce += aceleration[0] * Time.deltaTime;
        else reduce -= aceleration[1] * Time.deltaTime;

        //Make values be under damping parameter range
        reduce = Mathf.Clamp(reduce, MIN, MAX);

        //Apply to the virtual camera parameter
        return NormalizeToInterval(value, min, max);
    }

    private Vector3 LinearBezierCurve(float time, Vector3 P0, Vector3 P1)
    {
        return P0 + time * (P1 - P0);
    }

    private Vector3 QuadraticBezierCurve(float time, Vector3 P0, Vector3 P1, Vector3 P2)
    {
        float _time = 1 - time;
        return Mathf.Pow(_time, 2) * P0 + 2 * _time * time * P1 + Mathf.Pow(time, 2) * P2;
    }

    private Vector3 LinealCameraMovement(bool condition, float[] aceleration, ref float reduce, Vector3 P0, Vector3 P1)
    {
        //Apply the reduce to the actual value
        Vector3 value = LinearBezierCurve(reduce, P0, P1);

        //Increment or Decrement the reduce to make feel aceleration
        if (condition) reduce += aceleration[0] * Time.deltaTime;
        else reduce -= aceleration[1] * Time.deltaTime;

        //Make values be under damping parameter range
        reduce = Mathf.Clamp(reduce, MIN, MAX);

        //Apply to the virtual camera parameter
        return value;
    }

    private Vector3 QuadraticCameraMovement(bool condition, float[] aceleration, ref float reduce, Vector3 P0, Vector3 P1, Vector3 P2)
    {
        //Apply the reduce to the actual value
        Vector3 value = QuadraticBezierCurve(reduce, P0, P1, P2);

        //Increment or Decrement the reduce to make feel aceleration
        if (condition) reduce += aceleration[0] * Time.deltaTime;
        else reduce -= aceleration[1] *  Time.deltaTime;

        //Make values be under damping parameter range
        reduce = Mathf.Clamp(reduce, MIN, MAX);

        //Apply to the virtual camera parameter
        return value;
    }
}