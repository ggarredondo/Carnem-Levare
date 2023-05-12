using System;
using System.Threading.Tasks;
using System.Threading;

public class BlockedState : IState
{
    private readonly CharacterStateMachine stateMachine;
    private readonly Controller controller;
    private readonly CharacterStats stats;
    public event Action OnEnter, OnExit;

    private Hitbox hitbox;
    public void Set(in Hitbox hitbox) => this.hitbox = hitbox;
    private CancellationTokenSource cancellationTokenSource;

    public BlockedState(in CharacterStateMachine stateMachine, in Controller controller, in CharacterStats stats)
    {
        this.stateMachine = stateMachine;
        this.controller = controller;
        this.stats = stats;
    }

    public void Enter() 
    {
        stateMachine.enabled = false;
        stateMachine.hitNumber++;

        controller.OnHurt += Blocked;

        OnEnter?.Invoke();
        Recover();
    }
    public void Update() {}
    public void FixedUpdate() {}
    private async void Recover()
    {
        cancellationTokenSource = new CancellationTokenSource();

        try {
            await Task.Delay(
                TimeSpan.FromMilliseconds(stats.CalculateDisadvantage(hitbox.AdvantageOnBlock, stateMachine.hitNumber)), 
                cancellationTokenSource.Token);

            stateMachine.TransitionToWalkingOrBlocking();
        }
        catch {}
    }
    public void Exit() 
    {
        cancellationTokenSource?.Cancel();
        controller.OnHurt -= Blocked;
        OnExit?.Invoke();
    }

    private void Blocked(in Hitbox hitbox)
    {
        if (!hitbox.Unblockable && controller.isBlocking) stats.DamageStaminaBlocked(hitbox);
        else stats.DamageStamina(hitbox);
    }

    public ref readonly Hitbox Hitbox { get => ref hitbox; }
}
