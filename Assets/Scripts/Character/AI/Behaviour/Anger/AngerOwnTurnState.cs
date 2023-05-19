
public class AngerOwnTurnState : AIState
{
    private AIStateMachine aiFSM;
    private AIController controller;
    private GameKnowledge gameKnowledge;
    public AngerOwnTurnState(in AIStateMachine aiFSM, in AIController controller, in GameKnowledge gameKnowledge)
    {
        this.aiFSM = aiFSM;
        this.controller = controller;
        this.gameKnowledge = gameKnowledge;
    }

    public void Enter() {}
    public void Update() {}
    public void Exit() {}
}
