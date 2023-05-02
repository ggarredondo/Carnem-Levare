using System;
using System.Threading.Tasks;

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
    private async void Recover()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(hitbox.AdvantageOnHit));
        character.StateMachine.TransitionToMovement();
    }
    public void Exit()
    {
        character.Controller.OnHurt -= character.StateMachine.TransitionToHurt;
        OnExit?.Invoke();
    }
    
    public ref readonly IHit Hitbox { get => ref hitbox; }
}
