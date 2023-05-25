
public class AggresiveOpponentTurnState : AIState
{
    private AggresiveAIStateMachine aiFSM;
    private AIController controller;
    private GameKnowledge gameKnowledge;
    public AggresiveOpponentTurnState(in AggresiveAIStateMachine aiFSM, in AIController controller, in GameKnowledge gameKnowledge)
    {
        this.aiFSM = aiFSM;
        this.controller = controller;
        this.gameKnowledge = gameKnowledge;
    }

    public void Enter() {}
    public void Update() {}
    public void Exit() {}
}
