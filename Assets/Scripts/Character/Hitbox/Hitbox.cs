using UnityEngine;

public class Hitbox : MonoBehaviour, IHit, IBlocked
{
    private string hitSound, blockedSound;
    private float target, stagger, damage;
    private double advantageOnBlock, advantageOnHit;
    private bool unblockable;

    private void Start()
    {
        SetActive(false);
    }

    public void Set(string hitSound, string blockedSound, float stagger, float damage,
        double advantageOnBlock, double advantageOnHit, bool unblockable)
    {
        this.hitSound = hitSound;
        this.blockedSound = blockedSound;
        this.stagger = stagger;
        this.damage = damage;
        this.advantageOnBlock = advantageOnBlock;
        this.advantageOnHit = advantageOnHit;
        this.unblockable = unblockable;
    }

    public void SetActive(bool active)
    {
        transform.gameObject.SetActive(active);
    }

    public string HitSound { get => hitSound; }
    public string BlockedSound { get => blockedSound; }
    public void SetTarget(float target) => this.target = target;
    public float Target { get => target; }
    public float Stagger { get => stagger; }
    public float Damage { get => damage; }
    public double AdvantageOnBlock { get => advantageOnBlock; }
    public double AdvantageOnHit { get => advantageOnHit; }
    public void SetUnblockable(bool unblockable) => this.unblockable = unblockable;
    public bool Unblockable { get => unblockable; }
}
