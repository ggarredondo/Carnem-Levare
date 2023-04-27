using System;

public class WalkingState : IState
{
    private readonly Character character;
    public event Action OnEnter, OnExit;
    private Action<bool> transitionToBlocking;

    public WalkingState(in Character character)
    {
        this.character = character;
        transitionToBlocking = (bool done) => { if (done) this.character.ChangeState(this.character.BlockingState); };
    }

    public void Enter() 
    {
        character.Controller.OnDoBlock += transitionToBlocking;
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
        character.Controller.OnDoBlock -= transitionToBlocking;
        OnExit?.Invoke();
    }
}
