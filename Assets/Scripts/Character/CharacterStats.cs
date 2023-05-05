using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    [SerializeField] private float stamina, maxStamina;
    [SerializeField] private float characterDamage;

    [Tooltip("Percentage of stamina damage taken when blocking")]
    [SerializeField] [Range(0f, 1f)] private float blockingMultiplier;

    [Tooltip("How quickly time disadvantage decreases through consecutive hits (combo decay in ms x number of hits)")]
    [SerializeField] private float comboDecay = 200f;

    [SerializeField] [InitializationField] private float height = 1f, mass = 1f, drag;

    [SerializeField] protected List<Move> moveList;

    public void Initialize(in Character character, in Rigidbody rb)
    {
        stamina = maxStamina;
        character.transform.localScale *= height;
        rb.mass = mass;
        rb.drag = drag;
        Character charac = character;
        moveList.ForEach(move => move.Initialize(charac));
    }
    public void SubscribeEvents(CharacterStateMachine stateMachine)
    {
        stateMachine.HurtState.OnEnter += () => AddToStamina(-stateMachine.HurtState.Hitbox.Damage);
        stateMachine.BlockedState.OnEnter += () => AddToStamina(-stateMachine.BlockedState.Hitbox.Damage * blockingMultiplier);
    }

    public float CalculateAttackDamage(float baseDamage) 
    {
        return baseDamage + characterDamage;
    }
    public void AddToStamina(float addend) 
    {
        stamina = Mathf.Clamp(stamina + Mathf.Round(addend), 0f, maxStamina);
    }

    public ref readonly float Stamina { get => ref stamina; }
    public ref readonly float MaxStamina { get => ref maxStamina; }
    public ref readonly float BlockingMultiplier { get => ref blockingMultiplier; }
    public ref readonly float ComboDecay { get => ref comboDecay; }
    public ref readonly List<Move> MoveList { get => ref moveList; }
}
