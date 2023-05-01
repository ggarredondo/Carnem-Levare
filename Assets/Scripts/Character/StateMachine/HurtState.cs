using System;

public class HurtState : IState
{
    private readonly Character character;
    public event Action OnEnter, OnExit;

    public HurtState(in Character character) => this.character = character;

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
