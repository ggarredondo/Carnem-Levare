using System;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class HurtState : IState
{
    private readonly Character character;
    public event Action OnEnter, OnExit;
    private IHit hitbox;
    public void Set(IHit hitbox) => this.hitbox = hitbox;
    private CancellationTokenSource cancellationTokenSource;

    public HurtState(in Character character) => this.character = character;

    public void Enter() 
    {
        character.Controller.OnHurt += character.StateMachine.TransitionToHurt;
        OnEnter?.Invoke();
        Recover();
    }
    public void Update() {}
    public void FixedUpdate() {}
    private async void Recover()
    {
        cancellationTokenSource = new CancellationTokenSource();

        try {
            Debug.Log("Stunlocked for " + hitbox.AdvantageOnHit + " ms");
            await Task.Delay(TimeSpan.FromMilliseconds(hitbox.AdvantageOnHit), cancellationTokenSource.Token);
            Debug.Log("Finished Stunlock");
            character.StateMachine.TransitionToWalkingOrBlocking();
        }
        catch {}
    }
    public void Exit()
    {
        cancellationTokenSource?.Cancel();
        character.Controller.OnHurt -= character.StateMachine.TransitionToHurt;
        OnExit?.Invoke();
    }
    
    public ref readonly IHit Hitbox { get => ref hitbox; }
}
