using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private string hitSound, blockedSound;
    private float stagger, totalDamage;
    private double advantageOnBlock, advantageOnHit;
    private bool unblockable;

    private void Start()
    {
        SetActive(false);
    }

    public void Set(string hitSound, string blockedSound, float stagger, float totalDamage,
        double advantageOnBlock, double advantageOnHit, bool unblockable)
    {
        this.hitSound = hitSound;
        this.blockedSound = blockedSound;
        this.stagger = stagger;
        this.totalDamage = totalDamage;
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
    public float Stagger { get => stagger; }
    public float TotalDamage { get => totalDamage; }
    public double AdvantageOnBlock { get => advantageOnBlock; }
    public double AdvantageOnHit { get => advantageOnHit; }
    public bool Unblockable { get => unblockable; }
}
