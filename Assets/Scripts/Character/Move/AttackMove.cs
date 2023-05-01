using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Scriptable Objects/Move/AttackMove")]
public class AttackMove : Move
{
    [Header("Attack-specific Sound")]
    [SerializeField] private string hitSound;
    [SerializeField] private string blockedSound;

    [Header("Attack-specific Time Data (ms)")]
    [SerializeField] private double advantageOnBlock;
    [SerializeField] private double advantageOnHit;

    private enum Stagger : int { Light, Medium, Hard }
    [Header("Attack Values")]
    [SerializeField] private string hitboxSubtag;
    private Hitbox hitbox;
    [SerializeField] private float baseDamage;
    [SerializeField] private Stagger baseStagger;
    [SerializeField] private bool unblockable;

    public override void Initialize(in Character character)
    {
        hitbox = GameObject.FindWithTag(character.HitboxPrefix + hitboxSubtag).GetComponent<Hitbox>();
        base.Initialize(character);
    }

    public override void InitMove()
    {
        hitbox.Set(hitSound, 
            blockedSound, 
            (float)baseStagger,
            character.Stats.CalculateAttackDamage(baseDamage),
            advantageOnHit, 
            advantageOnBlock, 
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
