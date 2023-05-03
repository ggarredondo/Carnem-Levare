using System;

public class MoveState : IState
{
    private readonly Character character;
    public event Action<int> OnEnter;
    public event Action OnExit;

    public int moveIndex;

    public MoveState(in Character character) => this.character = character;

    public void Enter() 
    {
        character.Controller.OnHurt += character.StateMachine.TransitionToHurt;
        OnEnter?.Invoke(moveIndex);
    }
    public void Update() {}
    public void FixedUpdate() {}
    public void Exit()
    {
        character.Controller.OnHurt -= character.StateMachine.TransitionToHurt;
        OnExit?.Invoke();
    }
}
