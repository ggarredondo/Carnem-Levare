using System;

public class MoveState : IState
{
    private readonly Controller controller;
    private readonly CharacterStateMachine stateMachine;
    public event Action<int> OnEnterInteger;
    public event Action OnEnter, OnExit;

    private int moveListCount;
    public int moveIndex, bufferedMoveIndex = -1;
    public bool BUFFER_FLAG = false;

    public MoveState(in CharacterStateMachine stateMachine, in Controller controller, in CharacterStats stats)
    {
        this.stateMachine = stateMachine;
        this.controller = controller;
        moveListCount = stats.MoveList != null ? stats.MoveList.Count : 0;
    }

    public void Enter() 
    {
        stateMachine.enabled = false;
        controller.OnDoMove += BufferMove;
        controller.OnHurt += stateMachine.TransitionToHurt;
        stateMachine.TransitionToRecovery = stateMachine.TransitionToWalkingOrBlocking;
        OnEnterInteger?.Invoke(moveIndex);
        OnEnter?.Invoke();
    }
    public void Update() {}
    public void FixedUpdate() {}
    public void Exit()
    {
        bufferedMoveIndex = -1;
        controller.OnDoMove -= BufferMove;
        controller.OnHurt -= stateMachine.TransitionToHurt;
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
