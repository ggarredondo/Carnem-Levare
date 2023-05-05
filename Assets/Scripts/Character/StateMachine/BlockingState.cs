using System;

public class BlockingState : IState
{
    private readonly Controller controller;
    private readonly CharacterStateMachine stateMachine;
    private readonly CharacterMovement movement;
    public event Action OnEnter, OnExit;

    public BlockingState(in Character character)
    {
        controller = character.Controller;
        stateMachine = character.StateMachine;
        movement = character.Movement;
    }

    public void Enter()
    {
        stateMachine.enabled = true;
        controller.OnDoBlock += stateMachine.TransitionToWalkingOrBlocking;
        controller.OnDoMove += stateMachine.TransitionToMove;
        controller.OnHurt += stateMachine.TransitionToBlockedOrHurt;
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
        controller.OnHurt -= stateMachine.TransitionToBlockedOrHurt;
        OnExit?.Invoke();
    }
}
