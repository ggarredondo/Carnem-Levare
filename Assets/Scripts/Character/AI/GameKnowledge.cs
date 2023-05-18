using UnityEngine;

public class GameKnowledge
{
    private readonly CharacterStateMachine agentStateMachine, opponentStateMachine;
    private readonly Transform agentPosition, opponentPosition;

    public GameKnowledge(in CharacterStateMachine agentStateMachine, in CharacterStateMachine opponentStateMachine)
    {
        this.agentStateMachine = agentStateMachine;
        this.opponentStateMachine = opponentStateMachine;
        agentPosition = agentStateMachine.transform;
        opponentPosition = opponentStateMachine.transform;
    }
}
