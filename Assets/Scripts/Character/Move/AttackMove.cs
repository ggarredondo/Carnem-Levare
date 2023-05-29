using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Move/AttackMove")]
public class AttackMove : Move
{
    [Header("Attack-specific Sound")]
    [SerializeField] private string hitSound;
    [SerializeField] private string blockedSound;

    [Header("Attack-specific Camera Movement")]
    [SerializeField] private double hitShakeTime;
    [SerializeField] private float hitShakeIntensity;

    [Header("Attack-specific Time Data (ms)")]
    [SerializeField] private double blockStun;
    [SerializeField] private double hitStun;

    private enum Stagger : int { Light, Medium, Hard }
    private enum HitboxType : int { LeftFist, RightFist, LeftElbow, RightElbow }
    [Header("Attack Values")]
    [SerializeField] private HitboxType hitbox;
    private Hitbox currentHitbox;
    [SerializeField] private float baseDamage;
    [SerializeField] private Stagger animationStagger;
    [SerializeField] private bool unblockable;

    protected override void UpdateStringData()
    {
        stringData.Clear();
        stringData.Add("Name"); stringData.Add(moveName);
        stringData.Add("Damage"); stringData.Add(baseDamage.ToString());

        stringData.Add("Start Up"); stringData.Add((int)RelativeStartUp + " ms");
        stringData.Add("Active"); stringData.Add((int)RelativeActive + " ms");
        stringData.Add("Recovery"); stringData.Add((int)RelativeRecovery + " ms");

        stringData.Add("Advantage On Hit"); stringData.Add(((int) AdvantageOnHit).ToString());
        stringData.Add("Advantage On Block"); stringData.Add(((int) AdvantageOnBlock).ToString());
    }

    public override void InitMove(in CharacterStats stats)
    {
        currentHitbox = stats.HitboxList[(int) hitbox];

        currentHitbox.Set(hitSound,
            blockedSound,
            hitShakeTime,
            hitShakeIntensity,
            (float)animationStagger,
            stats.CalculateAttackDamage(baseDamage),
            blockStun,
            hitStun,
            unblockable);
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

    public float BaseDamage => baseDamage;
    public bool Unblockable => unblockable;

    public double BlockStun => blockStun;
    public double HitStun => hitStun;

    public double AdvantageOnBlock => blockStun - (RelativeActive + RelativeRecovery);
    public double AdvantageOnHit => hitStun - (RelativeActive + RelativeRecovery);
}
