using System;

public class BlockedState : IState
{
    private readonly Character character;
    public event Action OnEnter, OnExit;
    private IBlocked hitbox;
    public void Set(IBlocked hitbox) => this.hitbox = hitbox;

    public BlockedState(in Character character) => this.character = character;

    public void Enter() 
    {
        character.Controller.OnHurt += character.StateMachine.TransitionToBlockedOrHurt;
        OnEnter?.Invoke();
    }
    public void Update() {}
    public void FixedUpdate() {}
    public void Exit() 
    {
        character.Controller.OnHurt -= character.StateMachine.TransitionToBlockedOrHurt;
        OnExit?.Invoke();
    }

    public ref readonly IBlocked Hitbox { get => ref hitbox; }
}
