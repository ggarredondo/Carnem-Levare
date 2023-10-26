
public class RNG
{
    private System.Random random;

    public RNG() => random = new System.Random();
    public RNG(int seed) => random = new System.Random(seed);

    /// <summary>
    /// Lower and upper bounds are both inclusive.
    /// </summary>
    public int RangeInt(int minValue, int maxValue) => random.Next(minValue, maxValue+1);

    /// <summary>
    /// Lower and upper bounds are both inclusive.
    /// </summary>
    public float RangeFloat(float minValue, float maxValue) => ((float)random.NextDouble() * (maxValue - minValue)) + minValue;

    /// <summary>
    /// Lower and upper bounds are both inclusive.
    /// </summary>
    public double RangeDouble(double minValue, double maxValue) => (random.NextDouble() * (maxValue - minValue)) + minValue;
}
