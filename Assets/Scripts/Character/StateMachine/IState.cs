
public interface IState
{
    public void Enter(in Character character);
    public void Update(in Character character);
    public void FixedUpdate(in Character character);
    public void Exit(in Character character);
}
