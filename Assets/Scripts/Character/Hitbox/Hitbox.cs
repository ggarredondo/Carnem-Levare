using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private string hitSound, blockedSound;
    private int damageToHealth, damageToStamina;
    private float animationBodyTarget, hurtAnimation;
    private double blockStun, hitStun;
    private CameraEffectsData cameraShake, blockingCameraShake;
    private Vector3 knockbackOnHit, knockbackOnBlock;

    private void Start()
    {
        SetActive(false);
    }

    public void Set(string hitSound, string blockedSound, CameraEffectsData cameraShake, CameraEffectsData blockingCameraShake, float hurtAnimation, int damageToHealth, int damageToStamina,
        double blockStun, double hitStun, in Vector3 knockbackOnHit, in Vector3 knockbackOnBlock)
    {
        this.hitSound = hitSound;
        this.blockedSound = blockedSound;
        this.cameraShake = cameraShake;
        this.blockingCameraShake = blockingCameraShake;
        this.hurtAnimation = hurtAnimation;
        this.damageToHealth = damageToHealth;
        this.damageToStamina = damageToStamina;
        this.blockStun = blockStun;
        this.hitStun = hitStun;
        this.knockbackOnHit = knockbackOnHit;
        this.knockbackOnBlock = knockbackOnBlock;
    }

    public void SetActive(bool active)
    {
        transform.gameObject.SetActive(active);
    }

    public string HitSound { get => hitSound; }
    public string BlockedSound { get => blockedSound; }

    public CameraEffectsData CameraShake { get => cameraShake; }
    public CameraEffectsData BlockingCameraShake { get => blockingCameraShake; }
    public void SetAnimationBodyTarget(float animationBodyTarget) => this.animationBodyTarget = animationBodyTarget;
    public float AnimationBodyTarget { get => animationBodyTarget; }
    public float HurtAnimation { get => hurtAnimation; }
    public float DamageToHealth { get => damageToHealth; }
    public float DamageToStamina { get => damageToStamina; }
    public double BlockStun { get => blockStun; }
    public double HitStun { get => hitStun; }
    public ref readonly Vector3 KnockbackOnHit => ref knockbackOnHit;
    public ref readonly Vector3 KnockbackOnBlock => ref knockbackOnBlock;
}
