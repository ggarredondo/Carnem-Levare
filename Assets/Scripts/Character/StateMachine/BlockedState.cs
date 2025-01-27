using System;
using System.Collections;
using RefDelegates;

public class BlockedState : CharacterState
{
    private CharacterStateMachine stateMachine;
    private Controller controller;
    private CharacterStats stats;
    private CharacterMovement movement;

    public event Action OnEnter, OnExit;
    public event ActionIn<Hitbox> OnEnterHitbox;

    private Hitbox hitbox;
    public void Set(in Hitbox hitbox) => this.hitbox = hitbox;
    private IEnumerator coroutine;

    public void Reference(in CharacterStateMachine stateMachine, in Controller controller, in CharacterStats stats, in CharacterMovement movement)
    {
        this.stateMachine = stateMachine;
        this.controller = controller;
        this.stats = stats;
        this.movement = movement;
    }

    public void Enter() 
    {
        stateMachine.enabled = true;
        stateMachine.OnHurt += stats.BlockedDamage;
        OnEnter?.Invoke();
        OnEnterHitbox?.Invoke(hitbox);

        stateMachine.hitNumber++;
        coroutine = StateFunctions.Recover(stateMachine, stats.CalculateStun(hitbox.BlockedStun, stateMachine.hitNumber));
        stateMachine.StartCoroutine(coroutine);
        movement.PushCharacter(hitbox.BlockedKnockback);
    }
    public void Update() 
    {
        movement.MoveCharacter(controller.MovementVector, movement.BlockedDirectionSpeed);
    }
    public void FixedUpdate() 
    {
        movement.LookAtTarget();
    }
    public void Exit() 
    {
        stateMachine.StopCoroutine(coroutine);
        stateMachine.OnHurt -= stats.BlockedDamage;
        OnExit?.Invoke();
    }

    public ref readonly Hitbox Hitbox { get => ref hitbox; }
}
