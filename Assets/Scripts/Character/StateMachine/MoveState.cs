using System;

public class MoveState : CharacterState
{
    private readonly CharacterStateMachine stateMachine;
    private readonly Controller controller;
    private readonly CharacterStats stats;
    private readonly CharacterMovement movement;
    public event Action<int> OnEnterInteger;
    public event Action OnEnter, OnExit;

    private int moveListCount;
    public int moveIndex, bufferedMoveIndex = -1;
    public bool BUFFER_FLAG = false, TRACKING_FLAG = false;

    public MoveState(in CharacterStateMachine stateMachine, in Controller controller, in CharacterStats stats, in CharacterMovement movement)
    {
        this.stateMachine = stateMachine;
        this.controller = controller;
        this.stats = stats;
        this.movement = movement;
        moveListCount = stats.MoveList != null ? stats.MoveList.Count : 0;
    }

    public void Enter() 
    {
        stateMachine.enabled = true;
        stateMachine.hitNumber = -1;

        controller.OnDoMove += BufferMove;
        controller.OnHurt += stats.DamageStamina;
        stateMachine.TransitionToRecovery = stateMachine.TransitionToWalkingOrBlocking;

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
        bufferedMoveIndex = -1;
        controller.OnDoMove -= BufferMove;
        controller.OnHurt -= stats.DamageStamina;
        OnExit?.Invoke();
    }

    private void BufferMove(int moveIndex)
    {
        if (bufferedMoveIndex == -1 && BUFFER_FLAG && moveIndex >= 0 && moveIndex < moveListCount) {
            bufferedMoveIndex = moveIndex;
            stateMachine.TransitionToRecovery = () => stateMachine.TransitionToMove(moveIndex);
        }
    }
}
