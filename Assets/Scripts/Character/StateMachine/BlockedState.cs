using System;
using System.Collections;

public class BlockedState : IState
{
    private readonly CharacterStateMachine stateMachine;
    private readonly Controller controller;
    private readonly CharacterStats stats;
    public event Action OnEnter, OnExit;

    private Hitbox hitbox;
    public void Set(in Hitbox hitbox) => this.hitbox = hitbox;
    private IEnumerator coroutine;

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

        coroutine = StateFunctions.Recover(stats, stateMachine, hitbox.AdvantageOnBlock);
        stateMachine.StartCoroutine(coroutine);
    }
    public void Update() {}
    public void FixedUpdate() {}
    public void Exit() 
    {
        stateMachine.StopCoroutine(coroutine);
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
