using System;

public class AttackingState : IState
{
    private readonly Character character;
    public event Action<int> OnEnter;
    public event Action OnExit;

    public int moveIndex;

    public AttackingState(in Character character) => this.character = character;

    public void Enter() 
    {
        OnEnter?.Invoke(moveIndex);
    }
    public void Update() {}
    public void FixedUpdate() {}
    public void Exit()
    {
        OnExit?.Invoke();
    }
}
