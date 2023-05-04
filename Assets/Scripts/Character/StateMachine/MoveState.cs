using System;

public class MoveState : IState
{
    private readonly Character character;
    public event Action<int> OnEnter;
    public event Action OnExit;

    public int moveIndex, bufferedMoveIndex = -1;
    public bool BUFFER_FLAG = false;

    public MoveState(in Character character) => this.character = character;

    public void Enter() 
    {
        character.Controller.OnDoMove += BufferMove;
        character.Controller.OnHurt += character.StateMachine.TransitionToHurt;
        character.StateMachine.TransitionToRecovery = character.StateMachine.TransitionToWalkingOrBlocking;
        OnEnter?.Invoke(moveIndex);
    }
    public void Update() {}
    public void FixedUpdate() {}
    public void Exit()
    {
        bufferedMoveIndex = -1;
        character.Controller.OnDoMove -= BufferMove;
        character.Controller.OnHurt -= character.StateMachine.TransitionToHurt;
        OnExit?.Invoke();
    }

    private void BufferMove(int moveIndex)
    {
        if (bufferedMoveIndex == -1 && BUFFER_FLAG) {
            bufferedMoveIndex = moveIndex;
            character.StateMachine.TransitionToRecovery = () => character.StateMachine.TransitionToMove(moveIndex);
        }
    }
}
