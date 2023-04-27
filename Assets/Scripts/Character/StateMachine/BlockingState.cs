using System;

public class BlockingState : IState
{
    private readonly Character character;
    public event Action OnEnter, OnExit;
    private Action<bool> transitionToWalking;

    public BlockingState(in Character character)
    {
        this.character = character;
        transitionToWalking = (bool done) => { if (!done) this.character.ChangeState(this.character.WalkingState); };
    }

    public void Enter()
    {
        character.Controller.OnDoBlock += transitionToWalking;
        OnEnter?.Invoke();
    }
    public void Update()
    {
        character.Movement.MoveCharacter(character.Controller.MovementVector);
    }
    public void FixedUpdate()
    {
        character.Movement.LookAtTarget();
    }
    public void Exit()
    {
        character.Controller.OnDoBlock -= transitionToWalking;
        OnExit?.Invoke();
    }
}
