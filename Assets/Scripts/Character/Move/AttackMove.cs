using UnityEngine;

public enum HurtSide : int { Left = -1, Mid = 0, Right = 1 }
public enum HurtPower : int { LightHit, MediumHit, HardHit }
public enum HitboxType : int { LeftFist, RightFist, LeftElbow, RightElbow }

[System.Serializable]
public class HitStopData
{
    [SerializeField] private float attackerAnimSpeed, defenderAnimSpeed;
    [SerializeField] private double startTimeMS, lengthMS;
    [SerializeField] private float amplitude, frequency;

    public ref readonly float AttackerAnimSpeed => ref attackerAnimSpeed;
    public ref readonly float DefenderAnimSpeed => ref defenderAnimSpeed;
    public ref readonly double StartTimeMS => ref startTimeMS;
    public ref readonly double LengthMS => ref lengthMS;
    public ref readonly float Amplitude => ref amplitude;
    public ref readonly float Frequency => ref frequency;
}

[CreateAssetMenu(menuName = "Scriptable Objects/Move/AttackMove")]
public class AttackMove : Move
{
    [Header("Attack-specific Sound")]
    [SerializeField] private string hurtSound;
    [SerializeField] private string blockedSound;
    [SerializeField] private string staggerSound;
    [SerializeField] private string koSound;

    [Header("Attack-specific Particles")]
    [SerializeField] private GameObject hurtParticlesPrefab;
    [SerializeField] private GameObject blockedParticlesPrefab;
    [SerializeField] private GameObject staggerParticlesPrefab;
    [SerializeField] private GameObject koParticlesPrefab;

    [Header("Attack-specific Time Data (ms)")]
    [SerializeField] private double hurtStun;
    [SerializeField] private double blockedStun;

    [Header("Attack-specific Camera Movement")]
    [SerializeField] private CameraEffectsData hurtCameraShake;
    [SerializeField] private CameraEffectsData blockedCameraShake;
    [SerializeField] private CameraEffectsData staggerCameraShake;
    [SerializeField] private CameraEffectsData koCameraShake;

    [Header("Attack HitStop")]
    [SerializeField] private HitStopData hurtHitStop;
    [SerializeField] private HitStopData blockedHitStop;
    [SerializeField] private HitStopData staggerHitStop;
    [SerializeField] private HitStopData koHitStop;

    [Header("Attack Knockback")]
    [SerializeField] private Vector3 hurtKnockback;
    [SerializeField] private Vector3 blockedKnockback;

    [Header("Attack Values")]
    [SerializeField] private HitboxType hitbox;
    [SerializeField] private HurtSide hurtSide;
    [SerializeField] private HurtPower hurtPower;
    private Hitbox currentHitbox;
    [SerializeField] private int damageToHealth, damageToStamina;
    [SerializeField] private bool hyperarmor;

    protected override void UpdateStringData()
    {
        stringData?.Clear();
        stringData?.Add(""); stringData?.Add(moveName);
        stringData?.Add("Power"); stringData?.Add(damageToHealth.ToString());
        stringData?.Add("Inertia"); stringData?.Add(damageToStamina.ToString());

        stringData?.Add("Start Up"); stringData?.Add((int)RelativeStartUp + " ms");
        stringData?.Add("Active"); stringData?.Add((int)RelativeActive + " ms");
        stringData?.Add("Recovery"); stringData?.Add((int)RelativeRecovery + " ms");

        stringData?.Add("Adv. On Hit"); stringData?.Add((int) AdvantageOnHit + " ms");
        stringData?.Add("Adv. On Block"); stringData?.Add((int) AdvantageOnBlock + " ms");
    }

    public override void InitMove(in CharacterStats stats)
    {
        TRACKING_FLAG = true;
        stats.HYPERARMOR_FLAG = hyperarmor;

        currentHitbox = stats.HitboxList[(int) hitbox];
        currentHitbox.Set(this,
            stats.CalculateDamageToHealth(damageToHealth),
            stats.CalculateDamageToStamina(damageToStamina));
    }
    public override void ActivateMove(in CharacterStats stats)
    {
        TRACKING_FLAG = false;
        currentHitbox.SetActive(true);
    }
    public override void DeactivateMove(in CharacterStats stats) => currentHitbox.SetActive(false);
    public override void EndMove(in CharacterStats stats) => stats.HYPERARMOR_FLAG = false;

    public ref readonly string HurtSound => ref hurtSound;
    public ref readonly string BlockedSound => ref blockedSound;
    public ref readonly string StaggerSound => ref staggerSound;
    public ref readonly string KOSound => ref koSound;

    public ref readonly GameObject HurtParticlesPrefab => ref hurtParticlesPrefab;
    public ref readonly GameObject BlockedParticlesPrefab => ref blockedParticlesPrefab;
    public ref readonly GameObject StaggerParticlesPrefab => ref staggerParticlesPrefab;
    public ref readonly GameObject KoParticlesPrefab => ref koParticlesPrefab;

    public double HurtStun => hurtStun;
    public double BlockedStun => blockedStun;

    public double AdvantageOnHit => hurtStun - (RelativeActive + RelativeRecovery);
    public double AdvantageOnBlock => blockedStun - (RelativeActive + RelativeRecovery);

    public ref readonly CameraEffectsData HurtCameraShake => ref hurtCameraShake;
    public ref readonly CameraEffectsData BlockedCameraShake => ref blockedCameraShake;
    public ref readonly CameraEffectsData StaggerCameraShake => ref staggerCameraShake;
    public ref readonly CameraEffectsData KOCameraShake => ref koCameraShake;

    public ref readonly HitStopData HurtHitStop => ref hurtHitStop;
    public ref readonly HitStopData BlockedHitStop => ref blockedHitStop;
    public ref readonly HitStopData StaggerHitStop => ref staggerHitStop;
    public ref readonly HitStopData KOHitStop => ref koHitStop;

    public ref readonly Vector3 HurtKnockback => ref hurtKnockback;
    public ref readonly Vector3 BlockedKnockback => ref blockedKnockback;

    public HurtSide HurtSide => hurtSide;
    public HurtPower HurtPower => hurtPower;
}
