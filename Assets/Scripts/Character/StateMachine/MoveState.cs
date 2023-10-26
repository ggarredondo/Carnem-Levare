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

    private UnityEngine.Vector2 fixedVector;

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
        stats.AddToStamina(-currentMove.StaminaCost);

        currentMove.InitMove(stats);
        stateMachine.OnActivateMove += ActivateMove;
        stateMachine.OnDeactivateMove += DeactivateMove;
        stateMachine.OnEndMove += EndMove;

        fixedVector = controller.MovementVector.normalized;

        OnEnterInteger?.Invoke(moveIndex);
        OnEnter?.Invoke();
    }
    public void Update()
    {
        movement.MoveCharacter(currentMove.FixedDirection ? fixedVector : controller.MovementVector, currentMove.DirectionSpeed);
    }
    public void FixedUpdate()
    {
        movement.LookAtTarget(currentMove.TRACKING_FLAG);
    }
    public void Exit()
    {
        stateMachine.OnHurt -= stats.HurtDamage;

        stateMachine.OnActivateMove -= ActivateMove;
        stateMachine.OnDeactivateMove -= DeactivateMove;
        stateMachine.OnEndMove -= EndMove;

        // In case the state exits before the
        // animation events are triggered.
        currentMove.DeactivateMove(stats);
        currentMove.EndMove(stats);

        OnExit?.Invoke();
    }

    private void ActivateMove() => currentMove.ActivateMove(stats);
    private void DeactivateMove() => currentMove.DeactivateMove(stats);
    private void EndMove() => stateMachine.TransitionToWalkingOrBlocking();
}