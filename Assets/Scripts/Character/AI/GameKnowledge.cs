using UnityEngine;

public class GameKnowledge
{
    private readonly CharacterStats agentStats;
    private readonly CharacterStateMachine agentStateMachine, opponentStateMachine;
    private readonly Transform agentPosition, opponentPosition;

    public GameKnowledge(in CharacterStats agentStats, in CharacterStateMachine agentStateMachine, in CharacterStateMachine opponentStateMachine)
    {
        this.agentStats = agentStats;
        this.agentStateMachine = agentStateMachine;
        this.opponentStateMachine = opponentStateMachine;

        agentPosition = agentStateMachine.transform;
        opponentPosition = opponentStateMachine.transform;
    }
}
