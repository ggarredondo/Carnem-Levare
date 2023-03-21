using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private Power power;
    private float damage;
    private bool unblockable;
    private string hitSound, blockedSound;
    private float advantageOnBlock, advantageOnHit;
    [System.NonSerialized] public bool hitFlag;

    public void Set(Power power, float damage, bool unblockable, string hitSound, string blockedSound,
        float advantageOnBlock, float advantageOnHit) 
    {
        this.power = power;
        this.damage = damage;
        this.unblockable = unblockable;
        this.hitSound = hitSound;
        this.blockedSound = blockedSound;
        this.advantageOnBlock = advantageOnBlock;
        this.advantageOnHit = advantageOnHit;
    }

    public void Activate(bool activated) {
        transform.gameObject.SetActive(activated && !hitFlag);
    }

    // Gets
    public Power Power { get => power; }
    public float Damage { get => damage; }
    public bool Unblockable { get => unblockable; }
    public string HitSound { get => hitSound; }
    public string BlockedSound { get => blockedSound; }
    public float AdvantageOnBlock { get => advantageOnBlock; }
    public float AdvantageOnHit { get => advantageOnHit; }
}
