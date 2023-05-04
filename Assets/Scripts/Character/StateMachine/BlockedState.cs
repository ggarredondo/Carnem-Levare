using System;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

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
            Debug.Log("Stunlocked for " + hitbox.AdvantageOnBlock + " ms");
            await Task.Delay(TimeSpan.FromMilliseconds(hitbox.AdvantageOnBlock), cancellationTokenSource.Token);
            Debug.Log("Finished Stunlock");
            character.StateMachine.TransitionToWalkingOrBlocking();
        }
        catch {}
    }
    public void Exit() 
    {
        cancellationTokenSource?.Cancel();
        character.Controller.OnHurt -= character.StateMachine.TransitionToBlockedOrHurt;
        OnExit?.Invoke();
    }

    public ref readonly IBlocked Hitbox { get => ref hitbox; }
}
