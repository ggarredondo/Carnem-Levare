using UnityEngine;

public class WalkingState : IState
{
    public void Enter(Character character) {}
    public void Update(Character character) 
    {
        //character.Movement.MoveCharacter(direction);
    }
    public void FixedUpdate(Character character) 
    {
        character.Movement.LookAtTarget();
    }
    public void Exit(Character character) {}
}
