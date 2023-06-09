using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private string hitSound, blockedSound;
    private float animationBodyTarget, animationStagger, damage, hitShakeIntensity;
    private double blockStun, hitStun, hitShakeTime;
    private Vector3 knockback;
    private bool unblockable;

    private void Start()
    {
        SetActive(false);
    }

    public void Set(string hitSound, string blockedSound, double hitShakeTime, float hitShakeIntensity, float animationStagger, float damage,
        double blockStun, double hitStun, in Vector3 knockback, bool unblockable)
    {
        this.hitSound = hitSound;
        this.blockedSound = blockedSound;
        this.hitShakeTime = hitShakeTime;
        this.hitShakeIntensity = hitShakeIntensity;
        this.animationStagger = animationStagger;
        this.damage = damage;
        this.blockStun = blockStun;
        this.hitStun = hitStun;
        this.knockback = knockback;
        this.unblockable = unblockable;
    }

    public void SetActive(bool active)
    {
        transform.gameObject.SetActive(active);
    }

    public string HitSound { get => hitSound; }
    public string BlockedSound { get => blockedSound; }

    public double HitShakeTime { get => hitShakeTime; }
    public float HitShakeIntensity { get => hitShakeIntensity; }
    public void SetAnimationBodyTarget(float animationBodyTarget) => this.animationBodyTarget = animationBodyTarget;
    public float AnimationBodyTarget { get => animationBodyTarget; }
    public float AnimationStagger { get => animationStagger; }
    public float Damage { get => damage; }
    public double BlockStun { get => blockStun; }
    public double HitStun { get => hitStun; }
    public ref readonly Vector3 Knockback => ref knockback;
    public void SetUnblockable(bool unblockable) => this.unblockable = unblockable;
    public bool Unblockable { get => unblockable; }
}
