using System;

public class StaggerState : CharacterState
{
    public event Action OnEnter, OnExit;

    public void Reference() {}
    public void Enter() 
    {
        OnEnter?.Invoke();
    }
    public void Update() {}
    public void FixedUpdate() {}
    public void Exit()
    {
        OnExit?.Invoke();
    }
}
