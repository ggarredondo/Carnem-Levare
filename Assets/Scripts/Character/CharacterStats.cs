using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    private CharacterStateMachine stateMachine;

    [SerializeField] private int health, maxHealth;
    [SerializeField] private int stamina, maxStamina;
    [SerializeField] private int damageToHealth, damageToStamina;
    [System.NonSerialized] public bool HYPERARMOR_FLAG = false;

    [Tooltip("How quickly time stun decreases through consecutive hits given by decayFunction(comboDecay, number of hits)")]
    [SerializeField] private double comboDecay;
    [SerializeField] private double minStun;

    [SerializeField] [InitializationField] private float height = 1f, mass = 1f, drag;

    [SerializeField] private List<Move> moveList;
    [SerializeField] private List<Hitbox> hitboxList;
 
    [SerializeField] private bool noHurt;
    [SerializeField] private bool noDeath;

    public void Initialize(in Character character, in Rigidbody rb)
    {
        health = maxHealth;
        stamina = maxStamina;
        character.transform.localScale *= height;
        rb.mass = mass;
        rb.drag = drag;
    }
    public void Reference(in CharacterStateMachine stateMachine) => this.stateMachine = stateMachine;

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
            AddToHealth(-hitbox.DamageToHealth);
            if (health <= 0) stateMachine.TransitionToKO(hitbox);
            else stateMachine.TransitionToHurt(hitbox);
        }
    }
    public void BlockedDamage(in Hitbox hitbox)
    {
        if (!noHurt)
        {
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
