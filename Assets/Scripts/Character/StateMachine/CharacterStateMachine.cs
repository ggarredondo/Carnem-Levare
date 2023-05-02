using UnityEngine;
using System;
using RefDelegates;

public class CharacterStateMachine : MonoBehaviour
{
    private Character character;
    private IState currentState;
    private WalkingState walkingState;
    private BlockingState blockingState;
    private AttackingState attackingState;
    private HurtState hurtState;
    private BlockedState blockedState;

    public void Initialize()
    {
        character = GetComponent<Character>();
        walkingState = new WalkingState(character);
        blockingState = new BlockingState(character);
        attackingState = new AttackingState(character);
        hurtState = new HurtState(character);
        blockedState = new BlockedState(character);
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

    public ref readonly IState CurrentState { get => ref currentState; }
    public ref readonly WalkingState WalkingState { get => ref walkingState; }
    public ref readonly BlockingState BlockingState { get => ref blockingState; }
    public ref readonly AttackingState AttackingState { get => ref attackingState; }
    public ref readonly HurtState HurtState { get => ref hurtState; }
    public ref readonly BlockedState BlockedState { get => ref blockedState; }

    public void TransitionToWalking() => ChangeState(walkingState);
    public void TransitionToBlocking() => ChangeState(blockingState);
    public void TransitionToMovement() => ChangeState(character.Controller.isBlocking ? blockingState : walkingState);
    public void TransitionToAttacking(int moveIndex)
    {
        if (moveIndex >= 0 && moveIndex < character.Stats.MoveList.Count)
        {
            attackingState.moveIndex = moveIndex;
            ChangeState(attackingState);
        }
    }
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
    public void TransitionToBlockedOrHurt(in Hitbox hitbox)
    {
        if (hitbox.Unblockable || !character.Controller.isBlocking) TransitionToHurt(hitbox);
        else TransitionToBlocked(hitbox);
    }
}
