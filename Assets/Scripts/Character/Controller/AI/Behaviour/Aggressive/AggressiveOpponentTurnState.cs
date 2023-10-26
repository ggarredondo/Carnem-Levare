
public class AggressiveOpponentTurnState : AIState
{
    private AggressiveAIStateMachine aiFSM;
    private AIController controller;
    private GameKnowledge gameKnowledge;

    private CharacterStateMachine agentStateMachine;

    public AggressiveOpponentTurnState(in AggressiveAIStateMachine aiFSM, in AIController controller, in GameKnowledge gameKnowledge)
    {
        this.aiFSM = aiFSM;
        this.controller = controller;
        this.gameKnowledge = gameKnowledge;

        agentStateMachine = gameKnowledge.AgentStateMachine;
    }

    public void Enter() 
    {
        controller.PerformBlock(true);
        agentStateMachine.WalkingState.OnEnter += aiFSM.TransitionToNeutral;
        agentStateMachine.BlockingState.OnEnter += aiFSM.TransitionToNeutral;
    }
    public void React() {}
    public void Exit() 
    {
        agentStateMachine.WalkingState.OnEnter -= aiFSM.TransitionToNeutral;
        agentStateMachine.BlockingState.OnEnter -= aiFSM.TransitionToNeutral;
    }
}
