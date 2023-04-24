using System;

public class WalkingState : IState
{
    public event Action OnEnter, OnExit;

    public void Enter(in Character character) 
    {
        character.Controller.OnDoBlock += character.TransitionToBlocking;
        OnEnter?.Invoke();
    }
    public void Update(in Character character) 
    {
        character.Movement.MoveCharacter(character.Controller.MovementVector);
    }
    public void FixedUpdate(in Character character) 
    {
        character.Movement.LookAtTarget();
    }
    public void Exit(in Character character) 
    {
        character.Controller.OnDoBlock -= character.TransitionToBlocking;
        OnExit?.Invoke();
    }
}
