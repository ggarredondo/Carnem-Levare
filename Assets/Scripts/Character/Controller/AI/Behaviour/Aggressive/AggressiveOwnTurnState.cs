using System.Collections.Generic;

public class AggressiveOwnTurnState : AIState
{
    private AggressiveAIStateMachine aiFSM;
    private AIController controller;
    private GameKnowledge gameKnowledge;

    private RNG sequenceRNG;
    private CharacterStateMachine agentStateMachine;
    private List<MoveSequence> sequences;
    private int selectedSequence, selectedMove, sequencesCount;

    public AggressiveOwnTurnState(in AggressiveAIStateMachine aiFSM, in AIController controller, in GameKnowledge gameKnowledge)
    {
        this.aiFSM = aiFSM;
        this.controller = controller;
        this.gameKnowledge = gameKnowledge;

        sequenceRNG = new RNG(GameManager.RANDOM_SEED);
        agentStateMachine = gameKnowledge.AgentStateMachine;
        sequences = controller.MoveSequences;
        sequencesCount = sequences.Count;
    }

    public void Enter() 
    {
        InitializeMove();
        agentStateMachine.WalkingState.OnEnter += NextMove;
        agentStateMachine.BlockingState.OnEnter += NextMove;
        agentStateMachine.HurtState.OnEnter += aiFSM.TransitionToOpponentTurn;
        NextMove();
    }
    public void React()
    {
        if (gameKnowledge.Distance > aiFSM.MinDistanceToOpponent)
            aiFSM.TransitionToNeutral();
    }
    public void Exit()
    {
        agentStateMachine.WalkingState.OnEnter -= NextMove;
        agentStateMachine.BlockingState.OnEnter -= NextMove;
        agentStateMachine.HurtState.OnEnter -= aiFSM.TransitionToOpponentTurn;
    }

    private void InitializeMove()
    {
        selectedSequence = sequenceRNG.RangeInt(0, sequencesCount - 1);
        selectedMove = 0;
    }
    private void NextMove()
    {
        controller.PerformMove(sequences[selectedSequence][selectedMove]);
        selectedMove++;
        if (selectedMove >= sequences[selectedSequence].Count)
            InitializeMove();
    }
}
