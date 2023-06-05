using System;
using System.Collections;

public class BlockedState : CharacterState
{
    private readonly CharacterStateMachine stateMachine;
    private readonly Controller controller;
    private readonly CharacterStats stats;
    private readonly CharacterMovement movement;

    public event Action OnEnter, OnExit;

    private Hitbox hitbox;
    public void Set(in Hitbox hitbox) => this.hitbox = hitbox;
    private IEnumerator coroutine;

    public BlockedState(in CharacterStateMachine stateMachine, in Controller controller, in CharacterStats stats, in CharacterMovement movement)
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
    }
    public void Update() 
    {
        movement.MoveCharacter(controller.MovementVector, movement.BlockedDirectionSpeed);
    }
    public void FixedUpdate() 
    {
        movement.LookAtTarget(!movement.IsIdle);
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
