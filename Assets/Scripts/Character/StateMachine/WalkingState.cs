
public class WalkingState : IState
{
    public void Enter(in Character character) {}
    public void Update(in Character character) 
    {
        character.Movement.MoveCharacter(character.Controller.MovementVector);
    }
    public void FixedUpdate(in Character character) 
    {
        character.Movement.LookAtTarget();
    }
    public void Exit(in Character character) {}
}
