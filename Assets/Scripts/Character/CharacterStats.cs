using System.Collections.Generic;
using UnityEngine;
using RefDelegates;

[System.Serializable]
public class CharacterStats
{
    private CharacterStateMachine stateMachine;

    [Header("Resistance Values")]
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private int stamina, maxStamina;
    [SerializeField] private int damageToHealth, damageToStamina;

    [Header("Stun Values")]
    [Tooltip("How quickly time stun decreases through consecutive hits given by decayFunction(comboDecay, number of hits)")]
    [SerializeField] private double comboDecay;
    [SerializeField] private double minStun;
    [SerializeField] private double staggerStun;

    [Header("Flags")]
    public bool HYPERARMOR_FLAG = false;
    public bool NOHURT_FLAG = false;
    public bool NODEATH_FLAG = false;

    [Header("Physical Attributes")]
    [SerializeField] private float height = 1f;
    [SerializeField] private float mass = 1f, drag = 0f;

    [Header("Lists")]
    [SerializeField] private List<Move> moveList;
    [SerializeField] private List<Hitbox> hitboxList;

    public event ActionIn<Hitbox> OnHurtDamage, OnBlockedDamage;

    public void Initialize(in Transform characterTransform, in Rigidbody rb)
    {
        health = maxHealth;
        stamina = maxStamina;
        characterTransform.localScale = Vector3.one;
        characterTransform.localScale *= height;
        rb.mass = mass;
        rb.drag = drag;
    }
    public void Reference(in CharacterStateMachine stateMachine) => this.stateMachine = stateMachine;

    public void OnValidate(in Transform characterTransform, in Rigidbody rb)
    {
        characterTransform.localScale = Vector3.one;
        characterTransform.localScale *= height;
        rb.mass = mass;
        rb.drag = drag;
    }

    public int CalculateDamageToHealth(int moveDmgToHealth) => damageToHealth + moveDmgToHealth;
    public int CalculateDamageToStamina(int moveDmgToStamina) => damageToStamina + moveDmgToStamina;

    public double CalculateStun(double stun, int hitNumber) => StepDecay(stun, hitNumber);
    public double StepDecay(double stun, int hitNumber) => System.Math.Max(minStun, stun - (hitNumber-1) * comboDecay);
    public double HitCounterDecay(double stun, int hitNumber) => hitNumber < comboDecay ? stun : minStun;

    private void AddToHealth(float addend) => health = (int) Mathf.Clamp(health + addend, 0f + System.Convert.ToSingle(NODEATH_FLAG), maxHealth);
    private void AddToStamina(float addend) => stamina = (int) Mathf.Clamp(stamina + addend, 0f, maxStamina);

    public void HurtDamage(in Hitbox hitbox)
    {
        if (!NOHURT_FLAG)
        {
            OnHurtDamage?.Invoke(hitbox);
            AddToHealth(-hitbox.DamageToHealth);
            AddToStamina(-hitbox.DamageToStamina);
            if (health <= 0) stateMachine.TransitionToKO(hitbox);
            else if (stamina <= 0) stateMachine.TransitionToStagger(hitbox);
            else if (!HYPERARMOR_FLAG) stateMachine.TransitionToHurt(hitbox);
        }
    }
    public void BlockedDamage(in Hitbox hitbox)
    {
        if (!NOHURT_FLAG)
        {
            OnBlockedDamage?.Invoke(hitbox);
            AddToStamina(-hitbox.DamageToStamina);
            if (stamina <= 0) stateMachine.TransitionToStagger(hitbox);
            else stateMachine.TransitionToBlocked(hitbox);
        }
    }

    public ref readonly int Health => ref health;
    public ref readonly int MaxHealth => ref maxHealth;
    public ref readonly int Stamina => ref stamina;
    public void ResetStamina() => stamina = maxStamina;
    public ref readonly int MaxStamina => ref maxStamina;
    public ref readonly double StaggerStun => ref staggerStun;
    public List<Move> MoveList { get => moveList;  set => moveList = value; }
    public ref readonly List<Hitbox> HitboxList => ref hitboxList;
}
