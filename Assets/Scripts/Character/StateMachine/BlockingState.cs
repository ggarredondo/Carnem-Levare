using System;

public class BlockingState : IState
{
    private readonly Character character;
    public event Action OnEnter, OnExit;

    public BlockingState(in Character character) => this.character = character;

    public void Enter()
    {
        character.Controller.OnDoBlock += character.StateMachine.TransitionToMovement;
        character.Controller.OnDoMove += character.StateMachine.TransitionToAttacking;
        OnEnter?.Invoke();
    }
    public void Update()
    {
        character.Movement.MoveCharacter(character.Controller.MovementVector);
    }
    public void FixedUpdate()
    {
        character.Movement.LookAtTarget();
    }
    public void Exit()
    {
        character.Controller.OnDoBlock -= character.StateMachine.TransitionToMovement;
        character.Controller.OnDoMove -= character.StateMachine.TransitionToAttacking;
        OnExit?.Invoke();
    }
}
