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
    }

    public float Distance => Vector3.Distance(agentTransform.position, opponentTransform.position);

    public ref readonly CharacterStats AgentStats => ref agentStats;
    public ref readonly CharacterStats OpponentStats => ref opponentStats;
    public ref readonly CharacterStateMachine AgentStateMachine => ref agentStateMachine;
    public ref readonly CharacterStateMachine OpponentStateMachine => ref opponentStateMachine;
}
