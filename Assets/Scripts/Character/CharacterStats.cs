using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    private CharacterStateMachine stateMachine;

    [SerializeField] private float stamina, maxStamina;
    [SerializeField] private float characterDamage;

    [Tooltip("Percentage of stamina damage taken when blocking")]
    [SerializeField] [Range(0f, 1f)] private float blockingMultiplier;

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
        stamina = maxStamina;
        character.transform.localScale *= height;
        rb.mass = mass;
        rb.drag = drag;
    }
    public void Reference(in CharacterStateMachine stateMachine) => this.stateMachine = stateMachine;

    public float CalculateAttackDamage(float baseDamage) => baseDamage + characterDamage;
    public double CalculateStun(double stun, float hitNumber) => StepDecay(stun, hitNumber);
    public double StepDecay(double stun, float hitNumber) => System.Math.Max(minStun, stun - (hitNumber-1) * comboDecay);
    public double HitCounterDecay(double stun, float hitNumber) => hitNumber < comboDecay ? stun : minStun;

    private void AddToStamina(float addend) => stamina = Mathf.Clamp(stamina + addend, 0f + System.Convert.ToSingle(noDeath), maxStamina);
    public void DamageStamina(in Hitbox hitbox)
    {
        if (!noHurt)
        {
            AddToStamina(-hitbox.Damage);
            if (stamina <= 0) stateMachine.TransitionToKO(hitbox);
            else stateMachine.TransitionToHurt(hitbox);
        }
    }
    public void DamageStaminaBlocked(in Hitbox hitbox)
    {
        if (!noHurt)
        {
            AddToStamina(Mathf.Round(-hitbox.Damage * blockingMultiplier));
            if (stamina <= 0) stateMachine.TransitionToKO(hitbox);
            else stateMachine.TransitionToBlocked(hitbox);
        }
    }

    public ref readonly float Stamina { get => ref stamina; }
    public ref readonly float MaxStamina { get => ref maxStamina; }
    public ref readonly List<Move> MoveList { get => ref moveList; }
    public ref readonly List<Hitbox> HitboxList { get => ref hitboxList; }
}
