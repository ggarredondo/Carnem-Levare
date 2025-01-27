
public class AggressiveNeutralState : AIState
{
    private AggressiveAIStateMachine aiFSM;
    private AIController controller;
    private GameKnowledge gameKnowledge;

    private CharacterStateMachine agentStateMachine;
    private RNG movementRNG;

    public AggressiveNeutralState(in AggressiveAIStateMachine aiFSM, in AIController controller, in GameKnowledge gameKnowledge)
    {
        this.aiFSM = aiFSM;
        this.controller = controller;
        this.gameKnowledge = gameKnowledge;

        agentStateMachine = gameKnowledge.AgentStateMachine;
        movementRNG = new RNG(GameManager.RANDOM_SEED);
    }

    public void Enter() 
    {
        controller.PerformBlock(false);
        controller.Movement(0f, 1f);

        agentStateMachine.HurtState.OnEnter += aiFSM.TransitionToOpponentTurn;
        agentStateMachine.StaggerState.OnEnter += aiFSM.TransitionToOpponentTurn;
    }
    public void React() 
    {
        if (gameKnowledge.Distance <= aiFSM.MinDistanceToOpponent)
        {
            controller.PerformBlock(true);
            controller.Movement(movementRNG.RangeInt(-1, 1), 0f);
            if (aiFSM.waitTimer < UnityEngine.Time.time)
                aiFSM.TransitionToOwnTurn();
        }
        else
        {
            controller.PerformBlock(false);
            controller.Movement(0f, 1f);
        }
    }
    public void Exit() 
    {
        agentStateMachine.HurtState.OnEnter -= aiFSM.TransitionToOpponentTurn;
        agentStateMachine.StaggerState.OnEnter -= aiFSM.TransitionToOpponentTurn;
    }
}
