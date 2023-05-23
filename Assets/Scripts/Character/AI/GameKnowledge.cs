using UnityEngine;

public class GameKnowledge
{
    private readonly CharacterStats agentStats, opponentStats;
    private readonly CharacterStateMachine agentStateMachine, opponentStateMachine;
    private readonly Transform agentTransform, opponentTransform;

    public GameKnowledge(in CharacterStats agentStats, in CharacterStats opponentStats, 
        in CharacterStateMachine agentStateMachine, in CharacterStateMachine opponentStateMachine)
    {
        this.agentStats = agentStats;
        this.opponentStats = opponentStats;
        this.agentStateMachine = agentStateMachine;
        this.opponentStateMachine = opponentStateMachine;

        agentTransform = agentStateMachine.transform;
        opponentTransform = opponentStateMachine.transform;
        UpdateImperfectKnowledge(0f);
    }

    public void UpdateImperfectKnowledge(float distanceError)
    {
        ImperfectDistance = Distance + distanceError;
    }

    public float Distance => Vector3.Distance(agentTransform.position, opponentTransform.position);
    public float ImperfectDistance { get; private set; }
}
