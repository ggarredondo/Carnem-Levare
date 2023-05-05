using System;

public class KOState : IState
{
    public event Action OnEnter, OnExit;

    public KOState() {}

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
