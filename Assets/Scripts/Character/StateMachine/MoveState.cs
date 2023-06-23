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

        stateMachine.OnHurt += stats.HurtDamage;
        currentMove = moveList[moveIndex];

        stateMachine.OnInitMove += InitMove;
        stateMachine.OnActivateMove += ActivateMove;
        stateMachine.OnDeactivateMove += DeactivateMove;
        stateMachine.OnEndMove += EndMove;

        OnEnterInteger?.Invoke(moveIndex);
        OnEnter?.Invoke();
    }
    public void Update() 
    {
        movement.MoveCharacter(controller.MovementVector, currentMove.DirectionSpeed);
    }
    public void FixedUpdate()
    {
        movement.LookAtTarget(currentMove.TRACKING_FLAG);
    }
    public void Exit()
    {
        stateMachine.OnHurt -= stats.HurtDamage;

        stateMachine.OnInitMove -= InitMove;
        stateMachine.OnActivateMove -= ActivateMove;
        stateMachine.OnDeactivateMove -= DeactivateMove;
        stateMachine.OnEndMove -= EndMove;

        // In case the state exits before the
        // animation events are triggered.
        currentMove.DeactivateMove(stats);
        currentMove.EndMove(stats);

        OnExit?.Invoke();
    }

    private void InitMove() => currentMove.InitMove(stats);
    private void ActivateMove() => currentMove.ActivateMove(stats);
    private void DeactivateMove() => currentMove.DeactivateMove(stats);
    private void EndMove() {
        currentMove.EndMove(stats);
        stateMachine.TransitionToWalkingOrBlocking();
    }
}