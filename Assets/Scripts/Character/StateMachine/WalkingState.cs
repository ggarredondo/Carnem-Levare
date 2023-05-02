using System;

public class WalkingState : IState
{
    private readonly Character character;
    public event Action OnEnter, OnExit;

    public WalkingState(in Character character) => this.character = character;

    public void Enter()
    {
        character.Controller.OnDoBlock += character.StateMachine.TransitionToMovement;
        character.Controller.OnDoMove += character.StateMachine.TransitionToAttacking;
        Hurtbox.OnHurt += character.StateMachine.TransitionToHurt;
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
        Hurtbox.OnHurt -= character.StateMachine.TransitionToHurt;
        OnExit?.Invoke();
    }
}
