using UnityEngine;

public class GameKnowledge
{
    private readonly CharacterStats agentStats;
    private readonly CharacterStateMachine agentStateMachine, opponentStateMachine;
    private readonly Transform agentTransform, opponentTransform;

    public GameKnowledge(in CharacterStats agentStats, in CharacterStateMachine agentStateMachine, in CharacterStateMachine opponentStateMachine)
    {
        this.agentStats = agentStats;
        this.agentStateMachine = agentStateMachine;
        this.opponentStateMachine = opponentStateMachine;

        agentTransform = agentStateMachine.transform;
        opponentTransform = opponentStateMachine.transform;
    }

    public float Distance => Vector3.Distance(agentTransform.position, opponentTransform.position);
}
