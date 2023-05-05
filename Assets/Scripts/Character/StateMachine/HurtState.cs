using System;
using System.Threading.Tasks;
using System.Threading;

public class HurtState : IState
{
    private readonly Controller controller;
    private readonly CharacterStateMachine stateMachine;
    public event Action OnEnter, OnExit;

    private IHit hitbox;
    public void Set(IHit hitbox) => this.hitbox = hitbox;
    private CancellationTokenSource cancellationTokenSource;

    public HurtState(in Character character)
    {
        controller = character.Controller;
        stateMachine = character.StateMachine;
    }

    public void Enter() 
    {
        stateMachine.enabled = false;
        controller.OnHurt += stateMachine.TransitionToHurt;
        OnEnter?.Invoke();
        Recover();
    }
    public void Update() {}
    public void FixedUpdate() {}
    private async void Recover()
    {
        cancellationTokenSource = new CancellationTokenSource();

        try {
            await Task.Delay(TimeSpan.FromMilliseconds(hitbox.AdvantageOnHit), cancellationTokenSource.Token);
            stateMachine.TransitionToWalkingOrBlocking();
        }
        catch {}
    }
    public void Exit()
    {
        cancellationTokenSource?.Cancel();
        controller.OnHurt -= stateMachine.TransitionToHurt;
        OnExit?.Invoke();
    }
    
    public ref readonly IHit Hitbox { get => ref hitbox; }
}
