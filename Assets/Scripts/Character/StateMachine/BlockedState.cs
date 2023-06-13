using System;
using System.Collections;

public class BlockedState : CharacterState
{
    private CharacterStateMachine stateMachine;
    private Controller controller;
    private CharacterStats stats;
    private CharacterMovement movement;

    public event Action OnEnter, OnExit;

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
        stateMachine.OnHurt += Blocked;
        OnEnter?.Invoke();

        stateMachine.hitNumber++;
        coroutine = StateFunctions.Recover(stats, stateMachine, hitbox.BlockStun);
        stateMachine.StartCoroutine(coroutine);
        movement.PushCharacter(hitbox.KnockbackOnBlock);
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
        stateMachine.OnHurt -= Blocked;
        OnExit?.Invoke();
    }

    private void Blocked(in Hitbox hitbox)
    {
        if (!hitbox.Unblockable && controller.isBlocking) stats.DamageStaminaBlocked(hitbox);
        else stats.DamageStamina(hitbox);
    }

    public ref readonly Hitbox Hitbox { get => ref hitbox; }
}
