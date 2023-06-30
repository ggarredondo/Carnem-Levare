using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private string hitSound, blockedSound, staggerSound, koSound;
    private int damageToHealth, damageToStamina;
    private float hurtSide, hurtHeight, hurtPower;
    private double blockStun, hitStun;
    private HitStopData hurtHitStop, blockHitStop, staggerHitStop, koHitStop;
    private CameraEffectsData hitCameraShake, blockCameraShake, staggerCameraShake, koCameraShake;
    private Vector3 knockbackOnHit, knockbackOnBlock;

    private void Start()
    {
        SetActive(false);
    }

    public void Set(string hitSound, string blockedSound, string staggerSound, string koSound,
        in CameraEffectsData hitCameraShake, in CameraEffectsData blockCameraShake, 
        in CameraEffectsData staggerCameraShake, in CameraEffectsData koCameraShake,
        in HitStopData hurtHitStop, in HitStopData blockHitStop, 
        in HitStopData staggerHitStop, in HitStopData koHitStop,
        float hurtSide, float hurtPower, int damageToHealth, int damageToStamina,
        double blockStun, double hitStun, in Vector3 knockbackOnHit, in Vector3 knockbackOnBlock)
    {
        this.hitSound = hitSound;
        this.blockedSound = blockedSound;
        this.staggerSound = staggerSound;
        this.koSound = koSound;
        this.hitCameraShake = hitCameraShake;
        this.blockCameraShake = blockCameraShake;
        this.staggerCameraShake = staggerCameraShake;
        this.koCameraShake = koCameraShake;
        this.hurtHitStop = hurtHitStop;
        this.blockHitStop = blockHitStop;
        this.staggerHitStop = staggerHitStop;
        this.koHitStop = koHitStop;
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
    public string KoSound { get => koSound; }

    public ref readonly CameraEffectsData HitCameraShake => ref hitCameraShake;
    public ref readonly CameraEffectsData BlockCameraShake => ref blockCameraShake;
    public ref readonly CameraEffectsData StaggerCameraShake => ref staggerCameraShake;
    public ref readonly CameraEffectsData KOCameraShake => ref koCameraShake;
    public ref readonly HitStopData HurtHitStop => ref hurtHitStop;
    public ref readonly HitStopData BlockHitStop => ref blockHitStop;
    public ref readonly HitStopData StaggerHitStop => ref staggerHitStop;
    public ref readonly HitStopData KOHitStop => ref koHitStop;
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
