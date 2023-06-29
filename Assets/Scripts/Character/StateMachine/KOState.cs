using System;
using RefDelegates;

public class KOState : CharacterState
{
    public event Action OnEnter, OnExit;
    public event ActionIn<Hitbox> OnEnterHitbox;

    private Hitbox hitbox;
    public void Set(in Hitbox hitbox) => this.hitbox = hitbox;

    public void Reference(in CharacterStateMachine stateMachine) 
    {
        stateMachine.enabled = false;
    }

    public void Enter()
    {
        OnEnter?.Invoke();
        OnEnterHitbox?.Invoke(hitbox);
    }
    public void Update() {}
    public void FixedUpdate() {}
    public void Exit()
    {
        OnExit?.Invoke();
    }

    public ref readonly Hitbox Hitbox { get => ref hitbox; }
}
