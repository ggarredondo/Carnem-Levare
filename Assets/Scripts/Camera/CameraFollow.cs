using UnityEngine;
using Cinemachine;
using System;

public class CameraFollow : MonoBehaviour
{

    //Max value of the damping parameter (Virtual camera)
    const float MAX = 1;
    const float MIN = 0;

    private float holdingMinTime;

    [Header("Target Objects")]
    public PlayerController playerController;
    public CinemachineVirtualCamera vcam;

    [Header("Follow Parameters")]
    [Range(0f, 5f)] public float cameraAceleration;
    public Tuple<float> damping;
    public bool cameraRotation;

    private float reduceDamping;
    private CinemachineTransposer transposer;

    [Header("Noise Parameters")]
    [Range(0f, 5f)] public float noiseAceleration;
    public Tuple<float> frequency;
    public Tuple<float> amplitude;
    public bool cameraNoise;

    private float reduceAmplitude, reduceFrequency;
    private CinemachineBasicMultiChannelPerlin noiseTransposer;

    [Header("Dolly Zoom Parameters")]
    public Tuple<float> zoomAceleration;
    public Tuple<float> fieldOfView;
    public Vector3 offsetVariation;
    public bool cameraMovement;

    private float reduceFOV, reduceBlock;
    private Tuple<Vector3> dollyZoomPositions;

    [NonSerialized] public Move currentMove;

    private void Awake()
    {
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
        noiseTransposer = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        currentMove = playerController.rightNormalSlot;

        dollyZoomPositions.Item1 = transposer.m_FollowOffset;
        dollyZoomPositions.Item2 = dollyZoomPositions.Item1 + offsetVariation;
    }

    public void Initialized()
    {
        holdingMinTime = currentMove.getChargeLimit / currentMove.chargeLimitDivisor;
        zoomAceleration.Item1 = 1 / (currentMove.getChargeLimit - holdingMinTime);
    }

    private void LateUpdate()
    {
        if (Time.timeScale > 0f)
        {
            if (cameraRotation)
            {
                transposer.m_YawDamping = OscillateWithVelocity(playerController.getIsMoving, cameraAceleration, ref reduceDamping, damping, Exponential);
            }

            if (cameraMovement)
            {
                transposer.m_FollowOffset = CameraMovement(currentMove.getChargePhase == Move.ChargePhase.performing && currentMove.getDeltaTimer >= holdingMinTime && currentMove.getDeltaTimer <= currentMove.getChargeLimit,
                                                           zoomAceleration, ref reduceBlock, new Vector3[] { dollyZoomPositions.Item1, dollyZoomPositions.Item2 } , LinearBezierCurve);

                vcam.m_Lens.FieldOfView = OscillateWithVelocity(currentMove.getChargePhase == Move.ChargePhase.performing && currentMove.getDeltaTimer >= holdingMinTime && currentMove.getDeltaTimer <= currentMove.getChargeLimit,
                                                                zoomAceleration, ref reduceFOV, fieldOfView, Mathf.Sin);
            }

            if (cameraNoise)
            {
                noiseTransposer.m_FrequencyGain = OscillateWithVelocity(playerController.getIsMovement, noiseAceleration, ref reduceFrequency, frequency, Exponential);
                noiseTransposer.m_AmplitudeGain = OscillateWithVelocity(playerController.getIsMovement, noiseAceleration, ref reduceAmplitude, amplitude, Exponential);
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

    private Vector3 LinearBezierCurve(float time, Vector3[] P)
    {
        return P[0] + time * (P[1] - P[0]);
    }

    private Vector3 QuadraticBezierCurve(float time, Vector3[] P)
    {
        float _time = 1 - time;
        return Mathf.Pow(_time, 2) * P[0] + 2 * _time * time * P[1] + Mathf.Pow(time, 2) * P[2];
    }

    private float OscillateWithVelocity(bool condition, float aceleration, ref float reduce, Tuple<float> interval, Func<float, float> function)
    {
        //Apply the reduce to the actual value
        float value = MAX * function(reduce);

        //Increment or Decrement the reduce to make feel aceleration
        if (condition) reduce += aceleration * Time.deltaTime;
        else           reduce -= aceleration * Time.deltaTime;

        //Make values be under damping parameter range
        reduce = Mathf.Clamp(reduce, MIN, MAX);

        //Apply to the virtual camera parameter
        return NormalizeToInterval(value, interval.Item1, interval.Item2);   
    }

    private float OscillateWithVelocity(bool condition, Tuple<float> aceleration, ref float reduce, Tuple<float> interval, Func<float, float> function)
    {
        //Apply the reduce to the actual value
        float value = MAX * function(reduce);

        //Increment or Decrement the reduce to make feel aceleration
        if (condition) reduce += aceleration.Item1 * Time.deltaTime;
        else reduce -= aceleration.Item2 * Time.deltaTime;

        //Make values be under damping parameter range
        reduce = Mathf.Clamp(reduce, MIN, MAX);

        //Apply to the virtual camera parameter
        return NormalizeToInterval(value, interval.Item1, interval.Item2);
    }

    private Vector3 CameraMovement(bool condition, Tuple<float> aceleration, ref float reduce, Vector3[] P, Func<float, Vector3[], Vector3> BezierCurve)
    {
        //Apply the reduce to the actual value
        Vector3 value = BezierCurve(reduce, P);

        //Increment or Decrement the reduce to make feel aceleration
        if (condition) reduce += aceleration.Item1 * Time.deltaTime;
        else reduce -= aceleration.Item2 * Time.deltaTime;

        //Make values be under damping parameter range
        reduce = Mathf.Clamp(reduce, MIN, MAX);

        //Apply to the virtual camera parameter
        return value;
    }
}

[Serializable]
public struct Tuple<T>
{
    public T Item1;
    public T Item2;
}