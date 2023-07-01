using System;
using System.Collections;
using RefDelegates;

public class StaggerState : CharacterState
{
    private CharacterStateMachine stateMachine;
    private CharacterStats stats;
    private CharacterMovement movement;

    public event Action OnEnter, OnExit;
    public event ActionIn<Hitbox> OnEnterHitbox;

    private Hitbox hitbox;
    public void Set(in Hitbox hitbox) => this.hitbox = hitbox;
    private IEnumerator coroutine;

    public void Reference(in CharacterStateMachine stateMachine, in CharacterStats stats, in CharacterMovement movement) 
    {
        this.stateMachine = stateMachine;
        this.stats = stats;
        this.movement = movement;
    }
    public void Enter() 
    {
        stateMachine.enabled = true;
        stateMachine.OnHurt += stats.HurtDamage;
        OnEnter?.Invoke();
        OnEnterHitbox?.Invoke(hitbox);

        stats.ResetStamina();
        coroutine = StateFunctions.Recover(stateMachine, stats.StaggerStun);
        stateMachine.StartCoroutine(coroutine);
        movement.PushCharacter(hitbox.HurtKnockback);
    }
    public void Update() {}
    public void FixedUpdate() 
    {
        movement.LookAtTarget();
    }
    public void Exit()
    {
        stateMachine.StopCoroutine(coroutine);
        stateMachine.OnHurt -= stats.HurtDamage;
        OnExit?.Invoke();
    }

    public ref readonly Hitbox Hitbox => ref hitbox;
}
