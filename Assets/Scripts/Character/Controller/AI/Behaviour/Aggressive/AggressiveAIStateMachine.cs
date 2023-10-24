using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/AI Behaviour/Aggressive")]
public class AggressiveAIStateMachine : AIStateMachine
{
    [SerializeField] private float minDistanceToOpponent = 2f;

    public override void Reference(in AIController controller, in GameKnowledge gameKnowledge)
    {
        ownTurnState = new AggressiveOwnTurnState(this, controller, gameKnowledge);
        neutralState = new AggressiveNeutralState(this, controller, gameKnowledge);
        opponentTurnState = new AggressiveOpponentTurnState(this, controller, gameKnowledge);
        base.Reference(controller, gameKnowledge);
    }

    public ref readonly float MinDistanceToOpponent => ref minDistanceToOpponent;
}
