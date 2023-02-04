using System;
using UnityEngine;

public class CameraUtilities
{
    private const float MAX = 1;
    private const float MIN = 0;

    private static float NormalizeToInterval(float num, float min, float max)
    {
        return num * (max - min) + min;
    }

    public static float Exponential(float x)
    {
        return Mathf.Pow(x, 4);
    }

    public static Vector3 LinearBezierCurve(float time, Vector3[] P)
    {
        return P[0] + time * (P[1] - P[0]);
    }

    public static Vector3 QuadraticBezierCurve(float time, Vector3[] P)
    {
        float _time = 1 - time;
        return Mathf.Pow(_time, 2) * P[0] + 2 * _time * time * P[1] + Mathf.Pow(time, 2) * P[2];
    }

    public static float OscillateParameter(bool condition, Tuple<float> aceleration, ref float reduce, Tuple<float> interval, Func<float, float> function)
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

    public static Vector3 Movement(bool condition, Tuple<float> aceleration, ref float reduce, Vector3[] P, Func<float, Vector3[], Vector3> BezierCurve)
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
