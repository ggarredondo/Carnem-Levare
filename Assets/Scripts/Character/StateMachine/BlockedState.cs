using System;
using System.Threading.Tasks;
using System.Threading;

public class BlockedState : IState
{
    private readonly Character character;
    public event Action OnEnter, OnExit;
    private IBlocked hitbox;
    public void Set(IBlocked hitbox) => this.hitbox = hitbox;
    private CancellationTokenSource cancellationTokenSource;

    public BlockedState(in Character character) => this.character = character;

    public void Enter() 
    {
        cancellationTokenSource?.Cancel();
        character.Controller.OnHurt += character.StateMachine.TransitionToBlockedOrHurt;
        OnEnter?.Invoke();
        Recover();
    }
    public void Update() {}
    public void FixedUpdate() {}
    private async void Recover()
    {
        cancellationTokenSource = new CancellationTokenSource();

        try {
            await Task.Delay(TimeSpan.FromMilliseconds(hitbox.AdvantageOnBlock));
            character.StateMachine.TransitionToWalkingOrBlocking();
        }
        catch {}
    }
    public void Exit() 
    {
        character.Controller.OnHurt -= character.StateMachine.TransitionToBlockedOrHurt;
        OnExit?.Invoke();
    }

    public ref readonly IBlocked Hitbox { get => ref hitbox; }
}
