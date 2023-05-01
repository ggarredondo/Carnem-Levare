using System;

public class BlockedState : IState
{
    private readonly Character character;
    public event Action OnEnter, OnExit;

    public BlockedState(in Character character) => this.character = character;

    public void Enter() 
    {
        OnEnter?.Invoke();
    }
    public void Update() {}
    public void FixedUpdate() {}
    public void Exit() 
    {
        OnExit?.Invoke();
    }
}
