
public class AggresiveNeutralState : AIState
{
    private AggresiveAIStateMachine aiFSM;
    private AIController controller;
    private GameKnowledge gameKnowledge;
    public AggresiveNeutralState(in AggresiveAIStateMachine aiFSM, in AIController controller, in GameKnowledge gameKnowledge)
    {
        this.aiFSM = aiFSM;
        this.controller = controller;
        this.gameKnowledge = gameKnowledge;
    }

    public void Enter() 
    {
        controller.PerformBlock(false);
        controller.Movement(0f, 1f);
    }
    public void Update() 
    {
        if (gameKnowledge.ImperfectDistance <= aiFSM.MinDistanceToOpponent)
        {
            controller.Movement(0f, 0f);
            aiFSM.TransitionToOwnTurn();
        }
    }
    public void Exit() {}
}
