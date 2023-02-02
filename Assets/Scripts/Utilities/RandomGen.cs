using UnityEngine;
using Random = System.Random;

public class RandomGen : MonoBehaviour
{
    public static Random rand = new();

    public static float Float(Tuple<float> interval)
    {
        double val = (rand.NextDouble() * (interval.Item2 - interval.Item1) + interval.Item1);
        return (float)val;
    }

    public static int Int(int a, int b)
    {
        return rand.Next(a, b);
    }
}
