using System;
using System.Collections.Generic;

public class MoveState : CharacterState
{
    private CharacterStateMachine stateMachine;
    private Controller controller;
    private CharacterStats stats;
    private CharacterMovement movement;
    public event Action<int> OnEnterInteger;
    public event Action OnEnter, OnExit;

    public int moveIndex;
    private Move currentMove;
    private List<Move> moveList;
    private bool TRACKING_FLAG = false;

    public void Reference(in CharacterStateMachine stateMachine, in Controller controller, in CharacterStats stats, in CharacterMovement movement)
    {
        this.stateMachine = stateMachine;
        this.controller = controller;
        this.stats = stats;
        moveList = stats.MoveList;
        this.movement = movement;
    }

    public void Enter()
    {
        stateMachine.enabled = true;
        stateMachine.hitNumber = 0;
        stats.HYPERARMOR_FLAG = moveList[moveIndex].Hyperarmor;

        stateMachine.OnHurt += stats.HurtDamage;
        currentMove = moveList[moveIndex];

        stateMachine.OnInitMove += InitMove;
        stateMachine.OnActivateMove += ActivateMove;
        stateMachine.OnDeactivateMove += DeactivateMove;
        stateMachine.OnEndMove += EndMove;
        stateMachine.OnStartTracking += StartTracking;
        stateMachine.OnStopTracking += StopTracking;

        OnEnterInteger?.Invoke(moveIndex);
        OnEnter?.Invoke();
    }
    public void Update() 
    {
        movement.MoveCharacter(controller.MovementVector, currentMove.DirectionSpeed);
    }
    public void FixedUpdate()
    {
        movement.LookAtTarget(TRACKING_FLAG);
    }
    public void Exit()
    {
        stats.HYPERARMOR_FLAG = false;

        stateMachine.OnHurt -= stats.HurtDamage;

        stateMachine.OnInitMove -= InitMove;
        stateMachine.OnActivateMove -= ActivateMove;
        stateMachine.OnDeactivateMove -= DeactivateMove;
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