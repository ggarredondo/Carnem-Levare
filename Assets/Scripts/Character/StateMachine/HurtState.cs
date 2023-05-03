using System;
using System.Threading.Tasks;
using System.Threading;

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
        cancellationTokenSource?.Cancel();
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
            await Task.Delay(TimeSpan.FromMilliseconds(hitbox.AdvantageOnHit), cancellationTokenSource.Token);
            character.StateMachine.TransitionToWalkingOrBlocking();
        }
        catch {} 
    }
    public void Exit()
    {
        character.Controller.OnHurt -= character.StateMachine.TransitionToHurt;
        OnExit?.Invoke();
    }
    
    public ref readonly IHit Hitbox { get => ref hitbox; }
}
