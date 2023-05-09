using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Move/AttackMove")]
public class AttackMove : Move
{
    [Header("Attack-specific Sound")]
    [SerializeField] private string hitSound;
    [SerializeField] private string blockedSound;

    [Header("Attack-specifyc Camera Movement")]
    [SerializeField] private double hitShakeTime;
    [SerializeField] private float hitShakeIntensity;

    [Header("Attack-specific Time Data (ms)")]
    [SerializeField] private double advantageOnBlock;
    [SerializeField] private double advantageOnHit;

    private enum Stagger : int { Light, Medium, Hard }
    [Header("Attack Values")]
    [SerializeField] private string hitboxSubtag;
    private Hitbox hitbox;
    [SerializeField] private float baseDamage;
    [SerializeField] private Stagger animationStagger;
    [SerializeField] private bool unblockable;

    public override void Initialize(in Character character, in CharacterStats stats)
    {
        hitbox = GameObject.FindWithTag(character.HitboxPrefix + hitboxSubtag).GetComponent<Hitbox>();
        base.Initialize(character, stats);
    }

    public override void InitMove()
    {
        hitbox.Set(hitSound, 
            blockedSound,
            hitShakeTime,
            hitShakeIntensity,
            (float)animationStagger,
            stats.CalculateAttackDamage(baseDamage),
            advantageOnBlock,
            advantageOnHit, 
            unblockable);
    }
    public override void ActivateMove()
    {
        hitbox.SetActive(true);
    }
    public override void DeactivateMove()
    {
        hitbox.SetActive(false);
    }
    public override void RecoverFromMove() {}
}
