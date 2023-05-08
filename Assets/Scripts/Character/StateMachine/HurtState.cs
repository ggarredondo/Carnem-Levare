using System;
using System.Threading.Tasks;
using System.Threading;

public class HurtState : IState
{  
    private readonly CharacterStateMachine stateMachine;
    private readonly Controller controller;
    private readonly CharacterStats stats;
    public event Action OnEnter, OnExit;

    private Hitbox hitbox;
    public void Set(in Hitbox hitbox) => this.hitbox = hitbox;
    private CancellationTokenSource cancellationTokenSource;

    public HurtState(in CharacterStateMachine stateMachine, in Controller controller, in CharacterStats stats)
    {
        this.stateMachine = stateMachine;
        this.controller = controller;
        this.stats = stats;
    }

    public void Enter() 
    {
        stateMachine.enabled = false;
        controller.OnHurt += stats.DamageStamina;
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
        controller.OnHurt -= stats.DamageStamina;
        OnExit?.Invoke();
    }
    
    public ref readonly Hitbox Hitbox { get => ref hitbox; }
}
