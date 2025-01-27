using UnityEngine;
using System;
using RefDelegates;

public class CharacterStateMachine : MonoBehaviour
{
    private Controller controller;
    private CharacterStats stats;

    private CharacterState currentState;
    private WalkingState walkingState;
    private BlockingState blockingState;
    private MoveState moveState;
    private HurtState hurtState;
    private BlockedState blockedState;
    private StaggerState staggerState;
    private KOState koState;

    [NonSerialized] public int hitNumber = -1;

    public void Initialize()
    {
        walkingState = new WalkingState();
        blockingState = new BlockingState();
        moveState = new MoveState();
        hurtState = new HurtState();
        blockedState = new BlockedState();
        staggerState = new StaggerState();
        koState = new KOState();
    }

    public void Reference(in Controller controller, in CharacterStats stats, in CharacterMovement movement)
    {
        this.controller = controller;
        this.stats = stats;

        walkingState.Reference(this, controller, stats, movement);
        blockingState.Reference(this, controller, stats, movement);
        moveState.Reference(this, controller, stats, movement);
        hurtState.Reference(this, stats, movement);
        staggerState.Reference(this, stats, movement);
        blockedState.Reference (this, controller, stats, movement);
        koState.Reference(this, movement);
    }

    private void Update() => currentState.Update();
    private void FixedUpdate() => currentState.FixedUpdate();
    private void ChangeState(in CharacterState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    #region Animation Events

    public event Action OnInitMove, OnActivateMove, OnDeactivateMove, OnEndMove;
    private void InitMove() => OnInitMove?.Invoke();
    private void ActivateMove() => OnActivateMove?.Invoke();
    private void DeactivateMove() => OnDeactivateMove?.Invoke();
    private void EndMove() => OnEndMove?.Invoke();

    #endregion

    public ref readonly CharacterState CurrentState { get => ref currentState; }
    public ref readonly WalkingState WalkingState { get => ref walkingState; }
    public ref readonly BlockingState BlockingState { get => ref blockingState; }
    public ref readonly MoveState MoveState { get => ref moveState; }
    public ref readonly HurtState HurtState { get => ref hurtState; }
    public ref readonly BlockedState BlockedState { get => ref blockedState; }
    public ref readonly StaggerState StaggerState { get => ref staggerState; }
    public ref readonly KOState KOState { get => ref koState; }

    public void TransitionToWalking() => ChangeState(walkingState);
    public void TransitionToBlocking() => ChangeState(blockingState);
    public void TransitionToWalkingOrBlocking() => ChangeState(controller.isBlocking ? blockingState : walkingState);
    public void TransitionToMove(int moveIndex)
    {
        moveState.moveIndex = moveIndex;
        ChangeState(moveState);
    }
    public void SafeTransitionToMove(int moveIndex)
    {
        if (moveIndex >= 0 && moveIndex < stats.MoveList.Count)
            TransitionToMove(moveIndex);
    }
    public ActionIn<Hitbox> OnHurt;
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
    public void TransitionToStagger(in Hitbox hitbox)
    {
        staggerState.Set(hitbox);
        ChangeState(staggerState);
    }
    public void TransitionToKO(in Hitbox hitbox)
    {
        koState.Set(hitbox);
        ChangeState(koState);
    }
}
