using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private Power power;
    private float damage;
    private bool unblockable;
    private string hitSound;
    [System.NonSerialized] public bool hitFlag;

    public void Set(Power power, float damage, bool unblockable, string hitSound) {
        this.power = power;
        this.damage = damage;
        this.unblockable = unblockable;
        this.hitSound = hitSound;
    }

    public void Activate(bool activated) {
        transform.gameObject.SetActive(activated && !hitFlag);
    }

    public Power Power { get => power; }
    public float Damage { get => damage; }
    public bool Unblockable { get => unblockable; }
    public string HitSound { get => hitSound; }
}
