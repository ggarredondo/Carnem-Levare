using System;
using System.Threading.Tasks;

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
        Recover();
    }
    public void Update() {}
    public void FixedUpdate() {}
    private async void Recover()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(hitbox.AdvantageOnBlock));
        character.StateMachine.TransitionToMovement();
    }
    public void Exit() 
    {
        character.Controller.OnHurt -= character.StateMachine.TransitionToBlockedOrHurt;
        OnExit?.Invoke();
    }

    public ref readonly IBlocked Hitbox { get => ref hitbox; }
}
