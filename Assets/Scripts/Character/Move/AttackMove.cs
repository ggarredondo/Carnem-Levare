using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Move/AttackMove")]
public class AttackMove : Move
{
    [Header("Attack-specific Sound")]
    [SerializeField] private string hitSound;
    [SerializeField] private string blockedSound;

    [Header("Attack-specific Camera Movement")]
    [SerializeField] private double targetShakeTime;
    [SerializeField] private float targetShakeIntensity;
    [SerializeField] private double screenShakeTime;
    [SerializeField] private float screenShakeFrequency;
    [SerializeField] private float screenShakeAmplitude;

    [Header("Attack-specific Time Data (ms)")]
    [SerializeField] private double blockStun;
    [SerializeField] private double hitStun;

    [Header("Attack Knockback")]
    [SerializeField] private Vector3 knockbackOnHit;
    [SerializeField] private Vector3 knockbackOnBlock;

    private enum HurtAnimation : int { LightHit, MediumHit, HardHit }
    private enum HitboxType : int { LeftFist, RightFist, LeftElbow, RightElbow }
    [Header("Attack Values")]
    [SerializeField] private HitboxType hitbox;
    [SerializeField] private HurtAnimation hurtAnimation;
    private Hitbox currentHitbox;
    [SerializeField] private int damageToHealth, damageToStamina;

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
        currentHitbox = stats.HitboxList[(int) hitbox];

        currentHitbox.Set(hitSound,
            blockedSound,
            targetShakeTime,
            targetShakeIntensity,
            screenShakeTime,
            screenShakeFrequency,
            screenShakeAmplitude,
            (float)hurtAnimation,
            stats.CalculateDamageToHealth(damageToHealth),
            stats.CalculateDamageToStamina(damageToStamina),
            blockStun,
            hitStun,
            knockbackOnHit,
            knockbackOnBlock);
    }
    public override void ActivateMove()
    {
        currentHitbox.SetActive(true);
    }
    public override void DeactivateMove()
    {
        currentHitbox.SetActive(false);
    }
    public override void EndMove() {}

    public float DamageToHealth => damageToHealth;
    public float DamageToStamina => damageToStamina;

    public double BlockStun => blockStun;
    public double HitStun => hitStun;

    public double AdvantageOnBlock => blockStun - (RelativeActive + RelativeRecovery);
    public double AdvantageOnHit => hitStun - (RelativeActive + RelativeRecovery);
}
