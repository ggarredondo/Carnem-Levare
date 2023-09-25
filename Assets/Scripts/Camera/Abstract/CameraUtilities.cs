using System;
using UnityEngine;

public class CameraUtilities
{
    public static float Lineal(float x)
    {
        return x;
    }

    public static float Exponential(float x)
    {
        return Mathf.Pow(x, 4);
    }

    public static Vector3 LinearBezierCurve(float time, Vector3[] P)
    {
        return P[0] + time * (P[1] - P[0]);
    }

    public static Vector3 LinearBezierCurve2(float time, Vector3[] P)
    {
        return P[0] + time * (P[1] - P[0]);
    }

    public static Vector3 QuadraticBezierCurve(float time, Vector3[] P)
    {
        float _time = 1 - time;
        return Mathf.Pow(_time, 2) * P[0] + 2 * _time * time * P[1] + Mathf.Pow(time, 2) * P[2];
    }
}

[Serializable]
public struct Tuple<T>
{
    public T Item1;
    public T Item2;
}

[Serializable]
public struct Tuple<T, K>
{
    public T Item1;
    public K Item2;
}
