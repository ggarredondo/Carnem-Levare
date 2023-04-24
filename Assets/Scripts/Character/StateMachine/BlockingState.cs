using System;

public class BlockingState : IState
{
    public event Action OnEnter, OnExit;

    public void Enter(in Character character)
    {
        character.Controller.OnStopBlock += character.TransitionToWalking;
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
        character.Controller.OnStopBlock -= character.TransitionToWalking;
        OnExit?.Invoke();
    }
}
