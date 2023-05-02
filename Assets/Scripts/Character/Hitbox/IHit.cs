
public interface IHit
{
    public string HitSound { get; }
    public float Target { get; }
    public float Stagger { get; }
    public float Damage { get; }
    public double AdvantageOnHit { get; }
}
