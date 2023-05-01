using System;

public class WalkingState : IState
{
    private readonly Character character;
    public event Action OnEnter, OnExit;

    public WalkingState(in Character character) => this.character = character;

    public void Enter() 
    {
        character.Controller.OnDoBlock += character.TransitionToMovement;
        character.Controller.OnDoMove += character.TransitionToAttacking;
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
        character.Controller.OnDoBlock -= character.TransitionToMovement;
        character.Controller.OnDoMove -= character.TransitionToAttacking;
        OnExit?.Invoke();
    }
}
