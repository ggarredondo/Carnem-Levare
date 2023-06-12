
public class AggresiveNeutralState : AIState
{
    private AggresiveAIStateMachine aiFSM;
    private AIController controller;
    private GameKnowledge gameKnowledge;

    private CharacterStateMachine agentStateMachine;

    public AggresiveNeutralState(in AggresiveAIStateMachine aiFSM, in AIController controller, in GameKnowledge gameKnowledge)
    {
        this.aiFSM = aiFSM;
        this.controller = controller;
        this.gameKnowledge = gameKnowledge;

        agentStateMachine = gameKnowledge.AgentStateMachine;
    }

    public void Enter() 
    {
        controller.PerformBlock(false);
        controller.Movement(0f, 1f);
        agentStateMachine.HurtState.OnEnter += aiFSM.TransitionToOpponentTurn;
    }
    public void React() 
    {
        if (gameKnowledge.Distance <= aiFSM.MinDistanceToOpponent)
        {
            controller.Movement(0f, 0f);
            aiFSM.TransitionToOwnTurn();
        }
    }
    public void Exit() 
    {
        agentStateMachine.HurtState.OnEnter -= aiFSM.TransitionToOpponentTurn;
    }
}
