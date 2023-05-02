using System;

public class HurtState : IState
{
    private readonly Character character;
    public event Action OnEnter, OnExit;
    private IHit hitbox;
    public void Set(IHit hitbox) => this.hitbox = hitbox;

    public HurtState(in Character character) => this.character = character;

    public void Enter() 
    {
        character.Controller.OnHurt += character.StateMachine.TransitionToHurt;
        OnEnter?.Invoke();
    }
    public void Update() {}
    public void FixedUpdate() {}
    public void Exit()
    {
        character.Controller.OnHurt -= character.StateMachine.TransitionToHurt;
        OnExit?.Invoke();
    }

    public ref readonly IHit Hitbox { get => ref hitbox; }
}
