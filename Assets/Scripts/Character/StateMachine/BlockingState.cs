using System;

public class BlockingState : CharacterState
{
    private readonly CharacterStateMachine stateMachine;
    private readonly Controller controller;
    private readonly CharacterStats stats;
    private readonly CharacterMovement movement;
    public event Action OnEnter, OnExit;

    public BlockingState(in CharacterStateMachine stateMachine, in Controller controller, in CharacterStats stats, in CharacterMovement movement)
    {
        this.stateMachine = stateMachine;
        this.controller = controller;
        this.stats = stats;
        this.movement = movement;
    }

    public void Enter()
    {
        stateMachine.enabled = true;
        stateMachine.hitNumber = -1;

        controller.OnDoBlock += stateMachine.TransitionToWalkingOrBlocking;
        controller.OnDoMove += stateMachine.TransitionToMove;
        stateMachine.OnHurt += Blocked;

        OnEnter?.Invoke();
    }
    public void Update()
    {
        movement.MoveCharacter(controller.MovementVector, movement.BlockingDirectionSpeed);
    }
    public void FixedUpdate()
    {
        movement.LookAtTarget(!movement.IsIdle);
    }
    public void Exit()
    {
        controller.OnDoBlock -= stateMachine.TransitionToWalkingOrBlocking;
        controller.OnDoMove -= stateMachine.TransitionToMove;
        stateMachine.OnHurt -= Blocked;
        OnExit?.Invoke();
    }

    private void Blocked(in Hitbox hitbox) 
    {
        if (!hitbox.Unblockable) stats.DamageStaminaBlocked(hitbox);
        else stats.DamageStamina(hitbox);
    }
}
