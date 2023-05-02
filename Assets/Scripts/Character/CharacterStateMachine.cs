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

    private Action transitionToWalking, transitionToBlocking, transitionToMovement;
    private Action<int> transitionToAttacking;
    private ActionIn<Hitbox> transitionToHurt, transitionToBlocked, transitionToBlockedOrHurt;

    public void Initialize()
    {
        character = GetComponent<Character>();
        walkingState = new WalkingState(character);
        blockingState = new BlockingState(character);
        attackingState = new AttackingState(character);
        hurtState = new HurtState(character);
        blockedState = new BlockedState(character);

        transitionToWalking = () => ChangeState(walkingState);
        transitionToBlocking = () => ChangeState(blockingState);
        transitionToMovement = () => ChangeState(character.Controller.isBlocking ? blockingState : walkingState);
        transitionToAttacking = (int moveIndex) =>
        {
            if (moveIndex >= 0 && moveIndex < character.Stats.MoveList.Count)
            {
                attackingState.moveIndex = moveIndex;
                ChangeState(attackingState);
            }
        };
        transitionToHurt = (in Hitbox hitbox) => 
        {
            hurtState.Set(hitbox);
            ChangeState(hurtState);
        };
        transitionToBlocked = (in Hitbox hitbox) =>
        {
            blockedState.Set(hitbox);
            ChangeState(blockedState);
        };
        transitionToBlockedOrHurt = (in Hitbox hitbox) =>
        {
            if (hitbox.Unblockable || !character.Controller.isBlocking) transitionToHurt.Invoke(hitbox);
            else transitionToBlocked.Invoke(hitbox);
        };
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

    public ref readonly Action TransitionToWalking { get => ref transitionToWalking; }
    public ref readonly Action TransitionToBlocking { get => ref transitionToBlocking; }
    public ref readonly Action TransitionToMovement { get => ref transitionToMovement; }
    public ref readonly Action<int> TransitionToAttacking { get => ref transitionToAttacking; }
    public ref readonly ActionIn<Hitbox> TransitionToHurt { get => ref transitionToHurt; }
    public ref readonly ActionIn<Hitbox> TransitionToBlocked { get => ref transitionToBlocked; }
    public ref readonly ActionIn<Hitbox> TransitionToBlockedOrHurt { get => ref transitionToBlockedOrHurt; }
}
