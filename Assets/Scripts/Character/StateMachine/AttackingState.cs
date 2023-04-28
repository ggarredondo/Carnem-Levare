using System;
using UnityEngine;

public class AttackingState : IState
{
    private readonly Character character;
    private int moveIndex;
    public void SetMoveIndex(int moveIndex) => this.moveIndex = moveIndex;
    public event Action<int> OnEnter;
    public event Action OnExit;

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
