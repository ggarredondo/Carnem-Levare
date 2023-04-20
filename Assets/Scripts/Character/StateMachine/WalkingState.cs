using UnityEngine;

public class WalkingState : IState
{
    public void Enter(Character character) {}
    public void Update(Character character) {}
    public void FixedUpdate(Character character) 
    {
        character.Movement.LookAtTarget();
    }
    public void Exit(Character character) {}
}
