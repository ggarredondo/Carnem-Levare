using System;
using System.Collections;

public class HurtState : CharacterState
{  
    private readonly CharacterStateMachine stateMachine;
    private readonly CharacterStats stats;
    public event Action OnEnter, OnExit;

    private Hitbox hitbox;
    public void Set(in Hitbox hitbox) => this.hitbox = hitbox;
    private IEnumerator coroutine;

    public HurtState(in CharacterStateMachine stateMachine, in CharacterStats stats)
    {
        this.stateMachine = stateMachine;
        this.stats = stats;
    }

    public void Enter() 
    {
        stateMachine.enabled = false;
        stateMachine.hitNumber++;

        stateMachine.OnHurt += stats.DamageStamina;

        OnEnter?.Invoke();

        coroutine = StateFunctions.Recover(stats, stateMachine, hitbox.HitStun);
        stateMachine.StartCoroutine(coroutine);
    }
    public void Update() {}
    public void FixedUpdate() {}
    public void Exit()
    {
        stateMachine.StopCoroutine(coroutine);
        stateMachine.OnHurt -= stats.DamageStamina;
        OnExit?.Invoke();
    }
    
    public ref readonly Hitbox Hitbox { get => ref hitbox; }
}
