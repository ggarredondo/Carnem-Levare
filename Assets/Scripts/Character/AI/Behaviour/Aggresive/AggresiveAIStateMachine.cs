using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/AI Behaviour/Aggresive")]
public class AggresiveAIStateMachine : AIStateMachine
{
    [SerializeField] private float minDistanceToOpponent = 2f;

    public override void Reference(in AIController controller, in GameKnowledge gameKnowledge)
    {
        ownTurnState = new AggresiveOwnTurnState(this, controller, gameKnowledge);
        neutralState = new AggresiveNeutralState(this, controller, gameKnowledge);
        opponentTurnState = new AggresiveOpponentTurnState(this, controller, gameKnowledge);
        base.Reference(controller, gameKnowledge);
    }

    public ref readonly float MinDistanceToOpponent => ref minDistanceToOpponent;
}
