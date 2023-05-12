using UnityEngine;
using System;
using System.Collections.Generic;

public class CharacterStateMachine : MonoBehaviour
{
    private Controller controller;
    private CharacterStats stats;
    private List<Move> moveList;
    private Move currentMove;

    private IState currentState;
    private WalkingState walkingState;
    private BlockingState blockingState;
    private MoveState moveState;
    private HurtState hurtState;
    private BlockedState blockedState;
    private KOState koState;

    public void Reference(in Controller controller, in CharacterStats stats, in CharacterMovement movement)
    {
        this.controller = controller;
        this.stats = stats;
        moveList = stats.MoveList;

        walkingState = new WalkingState(this, controller, stats, movement);
        blockingState = new BlockingState(this, controller, stats, movement);
        moveState = new MoveState(this, controller, stats, movement);
        hurtState = new HurtState(this, controller, stats);
        blockedState = new BlockedState(this, controller, stats);
        koState = new KOState(this);
    }

    private void Update()
    {
        currentState.Update();
    }
    private void FixedUpdate()
    {
        currentState.FixedUpdate();
    }
    private void ChangeState(in IState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    #region Animation Events

    private void InitMove()
    {
        currentMove = moveList[moveState.moveIndex];
        currentMove.InitMove(stats);
    }
    private void ActivateMove() => currentMove.ActivateMove();
    private void DeactivateMove() => currentMove.DeactivateMove();
    private void EnableBuffering() => moveState.BUFFER_FLAG = true;
    private void EndMove()
    {
        moveState.BUFFER_FLAG = false;
        currentMove.RecoverFromMove();
        TransitionToRecovery.Invoke();
    }

    private void StartTracking() => moveState.TRACKING_FLAG = true;
    private void StopTracking() => moveState.TRACKING_FLAG = false;

    #endregion

    public ref readonly IState CurrentState { get => ref currentState; }
    public ref readonly WalkingState WalkingState { get => ref walkingState; }
    public ref readonly BlockingState BlockingState { get => ref blockingState; }
    public ref readonly MoveState MoveState { get => ref moveState; }
    public ref readonly HurtState HurtState { get => ref hurtState; }
    public ref readonly BlockedState BlockedState { get => ref blockedState; }
    public ref readonly KOState KOState { get => ref koState; }

    public void TransitionToWalking() => ChangeState(walkingState);
    public void TransitionToBlocking() => ChangeState(blockingState);
    public void TransitionToWalkingOrBlocking() => ChangeState(controller.isBlocking ? blockingState : walkingState);
    public void TransitionToMove(int moveIndex)
    {
        if (moveIndex >= 0 && moveIndex < stats.MoveList.Count)
        {
            moveState.moveIndex = moveIndex;
            ChangeState(moveState);
        }
    }
    public Action TransitionToRecovery;
    public void TransitionToHurt(in Hitbox hitbox)
    {
        hurtState.Set(hitbox);
        ChangeState(hurtState);
    }
    public void TransitionToBlocked(in Hitbox hitbox)
    {
        blockedState.Set(hitbox);
        ChangeState(blockedState);
    }
    public void TransitionToKO(in Hitbox hitbox)
    {
        koState.Set(hitbox);
        ChangeState(koState);
    }
}
