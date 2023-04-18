
public interface IState
{
    public void Enter(Character character);
    public void Update(Character character);
    public void FixedUpdate(Character character);
    public void Exit(Character character);
}
