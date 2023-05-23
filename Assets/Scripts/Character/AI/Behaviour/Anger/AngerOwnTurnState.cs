using System.Collections.Generic;

public class AngerOwnTurnState : AIState
{
    private AngerAIStateMachine aiFSM;
    private AIController controller;
    private GameKnowledge gameKnowledge;

    private CharacterStateMachine agentStateMachine;
    private List<MoveSequence> sequences;
    private int selectedSequence, selectedMove;

    public AngerOwnTurnState(in AngerAIStateMachine aiFSM, in AIController controller, in GameKnowledge gameKnowledge)
    {
        this.aiFSM = aiFSM;
        this.controller = controller;
        this.gameKnowledge = gameKnowledge;

        agentStateMachine = gameKnowledge.AgentStateMachine;
        sequences = controller.OwnTurnSequences;
    }

    public void Enter() 
    {
        //selectedMove = 0;
        //selectedSequence = 0;
        //controller.PerformMove(sequences[selectedSequence][selectedMove].MoveIndex);
        //selectedMove++;
        //agentStateMachine.OnEnableBuffering += () => controller.PerformMove(sequences[selectedSequence][selectedMove].MoveIndex);
    }
    public void Update() 
    {
        if (gameKnowledge.ImperfectDistance > aiFSM.MinDistanceToOpponent)
            aiFSM.TransitionToNeutral();
    }
    public void Exit() {}
}
