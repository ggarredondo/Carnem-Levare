using System.Collections.Generic;

public class AggresiveOwnTurnState : AIState
{
    private AggresiveAIStateMachine aiFSM;
    private AIController controller;
    private GameKnowledge gameKnowledge;

    private RNG sequenceRNG;
    private CharacterStateMachine agentStateMachine;
    private List<MoveSequence> sequences;
    private int selectedSequence, selectedMove, sequencesCount;

    public AggresiveOwnTurnState(in AggresiveAIStateMachine aiFSM, in AIController controller, in GameKnowledge gameKnowledge)
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
    }

    private void InitializeMove()
    {
        selectedSequence = sequenceRNG.RangeInt(0, sequencesCount - 1);
        selectedMove = 0;
    }
    private void NextMove()
    {
        controller.PerformMove(sequences[selectedSequence][selectedMove].MoveIndex);
        selectedMove++;
        if (selectedMove >= sequences[selectedSequence].Count)
            InitializeMove();
    }
}
