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
    public bool HYPERARMOR_FLAG = false;

    [Tooltip("How quickly time stun decreases through consecutive hits given by decayFunction(comboDecay, number of hits)")]
    [SerializeField] private double comboDecay;
    [SerializeField] private double minStun;

    [Header("Physical Attributes")]
    [SerializeField] private float height = 1f;
    [SerializeField] private float mass = 1f, drag = 0f;

    [Header("Lists")]
    [SerializeField] private List<Move> moveList;
    [SerializeField] private List<Hitbox> hitboxList;
 
    [SerializeField] private bool noHurt;
    [SerializeField] private bool noDeath;

    public event ActionIn<Hitbox> OnHurtDamage, OnBlockedDamage;

    public void Initialize(in Character character, in Rigidbody rb)
    {
        health = maxHealth;
        stamina = maxStamina;
        character.transform.localScale = Vector3.one;
        character.transform.localScale *= height;
        rb.mass = mass;
        rb.drag = drag;
    }
    public void Reference(in CharacterStateMachine stateMachine) => this.stateMachine = stateMachine;

    public void OnValidate(in Character character, in Rigidbody rb)
    {
        character.transform.localScale = Vector3.one;
        character.transform.localScale *= height;
        rb.mass = mass;
        rb.drag = drag;
    }

    public int CalculateDamageToHealth(int moveDmgToHealth) => damageToHealth + moveDmgToHealth;
    public int CalculateDamageToStamina(int moveDmgToStamina) => damageToStamina + moveDmgToStamina;

    public double CalculateStun(double stun, int hitNumber) => StepDecay(stun, hitNumber);
    public double StepDecay(double stun, int hitNumber) => System.Math.Max(minStun, stun - (hitNumber-1) * comboDecay);
    public double HitCounterDecay(double stun, int hitNumber) => hitNumber < comboDecay ? stun : minStun;

    private void AddToHealth(float addend) => health = (int) Mathf.Clamp(health + addend, 0f + System.Convert.ToSingle(noDeath), maxHealth);
    private void AddToStamina(float addend) => stamina = (int) Mathf.Clamp(stamina + addend, 0f, maxStamina);

    public void HurtDamage(in Hitbox hitbox)
    {
        if (!noHurt)
        {
            OnHurtDamage?.Invoke(hitbox);
            AddToHealth(-hitbox.DamageToHealth);
            if (health <= 0) stateMachine.TransitionToKO(hitbox);
            else if (!HYPERARMOR_FLAG) stateMachine.TransitionToHurt(hitbox);
        }
    }
    public void BlockedDamage(in Hitbox hitbox)
    {
        if (!noHurt)
        {
            OnBlockedDamage?.Invoke(hitbox);
            AddToStamina(-hitbox.DamageToStamina);
            //if (stamina <= 0) stateMachine.TransitionToStaggered(hitbox);
            //else stateMachine.TransitionToBlocked(hitbox);
            stateMachine.TransitionToBlocked(hitbox);
        }
    }

    public ref readonly int Health => ref health;
    public ref readonly int MaxHealth => ref maxHealth;
    public ref readonly int Stamina => ref stamina;
    public ref readonly int MaxStamina => ref maxStamina;
    public List<Move> MoveList { get => moveList;  set => moveList = value; }
    public ref readonly List<Hitbox> HitboxList => ref hitboxList;
}
