using System;
using System.Collections.Generic;

public class MoveState : CharacterState
{
    private readonly CharacterStateMachine stateMachine;
    private readonly Controller controller;
    private readonly CharacterStats stats;
    private readonly CharacterMovement movement;
    public event Action<int> OnEnterInteger;
    public event Action OnEnter, OnExit;

    public int moveIndex;
    private Move currentMove;
    private List<Move> moveList;
    private bool TRACKING_FLAG = false;

    private MoveBuffer moveBuffer;
    private double TIME_BEFORE_BUFFERING = 300.0;

    public MoveState(in CharacterStateMachine stateMachine, in Controller controller, in CharacterStats stats, in CharacterMovement movement)
    {
        this.stateMachine = stateMachine;
        this.controller = controller;
        this.stats = stats;
        moveList = stats.MoveList;
        this.movement = movement;
        moveBuffer = new MoveBuffer(TIME_BEFORE_BUFFERING, stateMachine, stats);
    }

    public void Enter() 
    {
        stateMachine.enabled = true;
        stateMachine.hitNumber = 0;

        moveBuffer.ListenTo(ref controller.OnDoMove);
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
        moveBuffer.StopListening(ref controller.OnDoMove);
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
        moveBuffer.NextTransition();
    }
    private void StartTracking() => TRACKING_FLAG = true;
    private void StopTracking() => TRACKING_FLAG = false;
}
