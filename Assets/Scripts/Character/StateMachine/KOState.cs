using System;

public class KOState : CharacterState
{
    public event Action OnEnter, OnExit;

    private Hitbox hitbox;
    public void Set(in Hitbox hitbox) => this.hitbox = hitbox;

    public KOState(in CharacterStateMachine stateMachine) 
    {
        stateMachine.enabled = false;
    }

    public void Enter()
    {
        OnEnter?.Invoke();
    }
    public void Update() {}
    public void FixedUpdate() {}
    public void Exit()
    {
        OnExit?.Invoke();
    }

    public ref readonly Hitbox Hitbox { get => ref hitbox; }
}
