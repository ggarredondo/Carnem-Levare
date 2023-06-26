using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private string hitSound, blockedSound, staggerSound;
    private int damageToHealth, damageToStamina;
    private float animationBodyTarget, hurtLevel;
    private double blockStun, hitStun;
    private CameraEffectsData hitCameraShake, blockCameraShake;
    private Vector3 knockbackOnHit, knockbackOnBlock;

    private void Start()
    {
        SetActive(false);
    }

    public void Set(string hitSound, string blockedSound, string staggerSound, 
        in CameraEffectsData hitCameraShake, in CameraEffectsData blockCameraShake, 
        float hurtLevel, int damageToHealth, int damageToStamina,
        double blockStun, double hitStun, in Vector3 knockbackOnHit, in Vector3 knockbackOnBlock)
    {
        this.hitSound = hitSound;
        this.blockedSound = blockedSound;
        this.staggerSound = staggerSound;
        this.hitCameraShake = hitCameraShake;
        this.blockCameraShake = blockCameraShake;
        this.hurtLevel = hurtLevel;
        this.damageToHealth = damageToHealth;
        this.damageToStamina = damageToStamina;
        this.blockStun = blockStun;
        this.hitStun = hitStun;
        this.knockbackOnHit = knockbackOnHit;
        this.knockbackOnBlock = knockbackOnBlock;
    }

    public void SetActive(bool active) => transform.gameObject.SetActive(active);

    public string HitSound { get => hitSound; }
    public string BlockedSound { get => blockedSound; }
    public string StaggerSound { get => staggerSound; }

    public ref readonly CameraEffectsData HitCameraShake => ref hitCameraShake;
    public ref readonly CameraEffectsData BlockCameraShake => ref blockCameraShake;
    public void SetAnimationBodyTarget(float animationBodyTarget) => this.animationBodyTarget = animationBodyTarget;
    public float AnimationBodyTarget { get => animationBodyTarget; }
    public float HurtLevel { get => hurtLevel; }
    public float DamageToHealth { get => damageToHealth; }
    public float DamageToStamina { get => damageToStamina; }
    public double BlockStun { get => blockStun; }
    public double HitStun { get => hitStun; }
    public ref readonly Vector3 KnockbackOnHit => ref knockbackOnHit;
    public ref readonly Vector3 KnockbackOnBlock => ref knockbackOnBlock;
}
