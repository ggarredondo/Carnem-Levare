using UnityEngine;

public class CharacterStateMachine : MonoBehaviour
{
    private Character character;
    private IState currentState;
    private WalkingState walkingState;
    private BlockingState blockingState;
    private MoveState moveState;
    private HurtState hurtState;
    private BlockedState blockedState;
    private KOState koState;

    public void Initialize()
    {
        character = GetComponent<Character>();
        walkingState = new WalkingState(character);
        blockingState = new BlockingState(character);
        moveState = new MoveState(character);
        hurtState = new HurtState(character);
        blockedState = new BlockedState(character);
        koState = new KOState(character);
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
    public ref readonly MoveState MoveState { get => ref moveState; }
    public ref readonly HurtState HurtState { get => ref hurtState; }
    public ref readonly BlockedState BlockedState { get => ref blockedState; }
    public ref readonly KOState KOState { get => ref koState; }

    public void TransitionToWalking() => ChangeState(walkingState);
    public void TransitionToBlocking() => ChangeState(blockingState);
    public void TransitionToWalkingOrBlocking() => ChangeState(character.Controller.isBlocking ? blockingState : walkingState);
    public void TransitionToMove(int moveIndex)
    {
        if (moveIndex >= 0 && moveIndex < character.Stats.MoveList.Count)
        {
            moveState.moveIndex = moveIndex;
            ChangeState(moveState);
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
