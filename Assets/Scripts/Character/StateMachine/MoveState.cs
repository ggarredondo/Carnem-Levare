using System;
using System.Collections.Generic;

public class MoveState : CharacterState
{
    private readonly CharacterStateMachine stateMachine;
    private readonly CharacterStats stats;
    private readonly CharacterMovement movement;
    public event Action<int> OnEnterInteger;
    public event Action OnEnter, OnExit;

    public int moveIndex;
    private Move currentMove;
    private List<Move> moveList;
    private bool TRACKING_FLAG = false;

    public MoveState(in CharacterStateMachine stateMachine, in CharacterStats stats, in CharacterMovement movement)
    {
        this.stateMachine = stateMachine;
        this.stats = stats;
        moveList = stats.MoveList;
        this.movement = movement;
    }

    public void Enter() 
    {
        stateMachine.enabled = true;
        stateMachine.hitNumber = 0;

        stateMachine.OnHurt += stats.DamageStamina;
        currentMove = moveList[moveIndex];

        stateMachine.OnInitMove += InitMove;
        stateMachine.OnActivateMove += ActivateMove;
        stateMachine.OnDeactiveMove += DeactivateMove;
        stateMachine.OnEndMove += EndMove;
        stateMachine.OnStartTracking += StartTracking;
        stateMachine.OnStopTracking += StopTracking;

        OnEnterInteger?.Invoke(moveIndex);
        OnEnter?.Invoke();
    }
    public void Update() {}
    public void FixedUpdate() 
    {
        movement.LookAtTarget(TRACKING_FLAG);
    }
    public void Exit()
    {
        stateMachine.OnHurt -= stats.DamageStamina;

        stateMachine.OnInitMove -= InitMove;
        stateMachine.OnActivateMove -= ActivateMove;
        stateMachine.OnDeactiveMove -= DeactivateMove;
        stateMachine.OnEndMove -= EndMove;
        stateMachine.OnStartTracking -= StartTracking;
        stateMachine.OnStopTracking -= StopTracking;

        // Disable Move and stop tracking in case
        // the animation event didn't trigger.
        DeactivateMove();
        StopTracking();

        OnExit?.Invoke();
    }

    private void InitMove() => currentMove.InitMove(stats);
    private void ActivateMove() => currentMove.ActivateMove();
    private void DeactivateMove() => currentMove.DeactivateMove();
    private void EndMove() {
        currentMove.EndMove();
        stateMachine.TransitionToWalkingOrBlocking();
    }
    private void StartTracking() => TRACKING_FLAG = true;
    private void StopTracking() => TRACKING_FLAG = false;
}
