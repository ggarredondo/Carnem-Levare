using System;
using System.Threading.Tasks;
using System.Threading;

public class BlockedState : IState
{
    private readonly Controller controller;
    private readonly CharacterStateMachine stateMachine;
    public event Action OnEnter, OnExit;

    private IBlocked hitbox;
    public void Set(IBlocked hitbox) => this.hitbox = hitbox;
    private CancellationTokenSource cancellationTokenSource;

    public BlockedState(in Character character)
    {
        controller = character.Controller;
        stateMachine = character.StateMachine;
    }

    public void Enter() 
    {
        stateMachine.enabled = false;
        controller.OnHurt += stateMachine.TransitionToBlockedOrHurt;
        OnEnter?.Invoke();
        Recover();
    }
    public void Update() {}
    public void FixedUpdate() {}
    private async void Recover()
    {
        cancellationTokenSource = new CancellationTokenSource();

        try {
            await Task.Delay(TimeSpan.FromMilliseconds(hitbox.AdvantageOnBlock), cancellationTokenSource.Token);
            stateMachine.TransitionToWalkingOrBlocking();
        }
        catch {}
    }
    public void Exit() 
    {
        cancellationTokenSource?.Cancel();
        controller.OnHurt -= stateMachine.TransitionToBlockedOrHurt;
        OnExit?.Invoke();
    }

    public ref readonly IBlocked Hitbox { get => ref hitbox; }
}
