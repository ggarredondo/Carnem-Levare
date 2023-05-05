using System;

public class WalkingState : IState
{
    private readonly Controller controller;
    private readonly CharacterStateMachine stateMachine;
    private readonly CharacterMovement movement;
    public event Action OnEnter, OnExit;

    public WalkingState(in CharacterStateMachine stateMachine, in Controller controller, in CharacterMovement movement)
    {
        this.stateMachine = stateMachine;
        this.controller = controller;
        this.movement = movement;
    }

    public void Enter()
    {
        stateMachine.enabled = true;
        controller.OnDoBlock += stateMachine.TransitionToWalkingOrBlocking;
        controller.OnDoMove += stateMachine.TransitionToMove;
        controller.OnHurt += stateMachine.TransitionToHurt;
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
        controller.OnHurt -= stateMachine.TransitionToHurt;
        OnExit?.Invoke();
    }
}
