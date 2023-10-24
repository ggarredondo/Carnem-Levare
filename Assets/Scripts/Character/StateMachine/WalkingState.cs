using System;

public class WalkingState : CharacterState
{
    private CharacterStateMachine stateMachine;
    private Controller controller;
    private CharacterStats stats;
    private CharacterMovement movement;
    public event Action OnEnter, OnExit;

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
        stateMachine.hitNumber = 0;

        controller.OnDoBlock += stateMachine.TransitionToWalkingOrBlocking;
        controller.OnDoMove += stateMachine.SafeTransitionToMove;
        stateMachine.OnHurt += stats.HurtDamage;

        OnEnter?.Invoke();
    }
    public void Update() 
    {
        movement.MoveCharacter(controller.MovementVector, movement.WalkingDirectionSpeed);
        stats.RegenStamina();
    }
    public void FixedUpdate() 
    {
        movement.LookAtTarget(!movement.IsIdle);
    }
    public void Exit() 
    {
        controller.OnDoBlock -= stateMachine.TransitionToWalkingOrBlocking;
        controller.OnDoMove -= stateMachine.SafeTransitionToMove;
        stateMachine.OnHurt -= stats.HurtDamage;
        OnExit?.Invoke();
    }
}
