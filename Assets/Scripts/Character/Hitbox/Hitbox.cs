using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private string hitSound, blockedSound, staggerSound;
    private int damageToHealth, damageToStamina;
    private float hurtSide, hurtHeight, hurtPower;
    private double blockStun, hitStun;
    private HitStopData hurtHitStop, blockHitStop, staggerHitStop;
    private CameraEffectsData hitCameraShake, blockCameraShake, staggerCameraShake;
    private Vector3 knockbackOnHit, knockbackOnBlock;

    private void Start()
    {
        SetActive(false);
    }

    public void Set(string hitSound, string blockedSound, string staggerSound,
        in CameraEffectsData hitCameraShake, in CameraEffectsData blockCameraShake, in CameraEffectsData staggerCameraShake,
        in HitStopData hurtHitStop, in HitStopData blockHitStop, in HitStopData staggerHitStop,
        float hurtSide, float hurtPower, int damageToHealth, int damageToStamina,
        double blockStun, double hitStun, in Vector3 knockbackOnHit, in Vector3 knockbackOnBlock)
    {
        this.hitSound = hitSound;
        this.blockedSound = blockedSound;
        this.staggerSound = staggerSound;
        this.hitCameraShake = hitCameraShake;
        this.blockCameraShake = blockCameraShake;
        this.staggerCameraShake = staggerCameraShake;
        this.hurtHitStop = hurtHitStop;
        this.blockHitStop = blockHitStop;
        this.staggerHitStop = staggerHitStop;
        this.hurtSide = hurtSide;
        this.hurtPower = hurtPower;
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
    public ref readonly CameraEffectsData StaggerCameraShake => ref staggerCameraShake;
    public ref readonly HitStopData HurtHitStop => ref hurtHitStop;
    public ref readonly HitStopData BlockHitStop => ref blockHitStop;
    public ref readonly HitStopData StaggerHitStop => ref staggerHitStop;
    public float HurtSide { get => hurtSide; }
    public float HurtHeight { get => hurtHeight; }
    public float HurtPower { get => hurtPower; }
    public void SetHeight(float height) => hurtHeight = height;
    public float DamageToHealth { get => damageToHealth; }
    public float DamageToStamina { get => damageToStamina; }
    public double BlockStun { get => blockStun; }
    public double HitStun { get => hitStun; }
    public ref readonly Vector3 KnockbackOnHit => ref knockbackOnHit;
    public ref readonly Vector3 KnockbackOnBlock => ref knockbackOnBlock;
}
