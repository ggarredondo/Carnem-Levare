using UnityEngine;

public class GameKnowledge
{
    private readonly CharacterStats agentStats, opponentStats;
    private readonly CharacterStateMachine agentStateMachine, opponentStateMachine;
    private readonly Transform agentTransform, opponentTransform;

    private Vector3 agentPosition, opponentPosition;

    public GameKnowledge(in CharacterStats agentStats, in CharacterStats opponentStats, 
        in CharacterStateMachine agentStateMachine, in CharacterStateMachine opponentStateMachine)
    {
        this.agentStats = agentStats;
        this.opponentStats = opponentStats;
        this.agentStateMachine = agentStateMachine;
        this.opponentStateMachine = opponentStateMachine;

        agentTransform = agentStateMachine.transform;
        opponentTransform = opponentStateMachine.transform;
        UpdateKnowledge();
    }

    public void UpdateKnowledge()
    {
        agentPosition = agentTransform.position;
        opponentPosition = opponentTransform.position;
    }

    public float Distance => Vector3.Distance(agentPosition, opponentPosition);
}
