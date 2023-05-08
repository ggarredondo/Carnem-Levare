using System;

public class WalkingState : IState
{
    private readonly CharacterStateMachine stateMachine;
    private readonly Controller controller;
    private readonly CharacterStats stats;
    private readonly CharacterMovement movement;
    public event Action OnEnter, OnExit;

    public WalkingState(in CharacterStateMachine stateMachine, in Controller controller, in CharacterStats stats, in CharacterMovement movement)
    {
        this.stateMachine = stateMachine;
        this.controller = controller;
        this.stats = stats;
        this.movement = movement;
    }

    public void Enter()
    {
        stateMachine.enabled = true;
        controller.OnDoBlock += stateMachine.TransitionToWalkingOrBlocking;
        controller.OnDoMove += stateMachine.TransitionToMove;
        controller.OnHurt += stats.DamageStamina;
        OnEnter?.Invoke();
    }
    public void Update() 
    {
        movement.MoveCharacter(controller.MovementVector);
    }
    public void FixedUpdate() 
    {
        movement.LookAtTarget();
    }
    public void Exit() 
    {
        controller.OnDoBlock -= stateMachine.TransitionToWalkingOrBlocking;
        controller.OnDoMove -= stateMachine.TransitionToMove;
        controller.OnHurt -= stats.DamageStamina;
        OnExit?.Invoke();
    }
}
