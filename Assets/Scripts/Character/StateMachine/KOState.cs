using System;
using RefDelegates;

public class KOState : CharacterState
{
    private CharacterStateMachine stateMachine;
    private CharacterMovement movement;

    public event Action OnEnter, OnExit;
    public event ActionIn<Hitbox> OnEnterHitbox;

    private Hitbox hitbox;
    public void Set(in Hitbox hitbox) => this.hitbox = hitbox;

    public void Reference(in CharacterStateMachine stateMachine, in CharacterMovement movement) 
    {
        this.stateMachine = stateMachine;
        this.movement = movement;
    }

    public void Enter()
    {
        stateMachine.enabled = false;
        OnEnter?.Invoke();
        OnEnterHitbox?.Invoke(hitbox);
        movement.PushCharacter(hitbox.HurtKnockback);
    }
    public void Update() {}
    public void FixedUpdate() {}
    public void Exit()
    {
        OnExit?.Invoke();
    }

    public ref readonly Hitbox Hitbox { get => ref hitbox; }
}
