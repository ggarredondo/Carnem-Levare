using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/AI Behaviour/Anger")]
public class AngerAIStateMachine : AIStateMachine
{
    [SerializeField] private float minDistanceToOpponent = 2f;

    public override void Reference(in AIController controller, in GameKnowledge gameKnowledge)
    {
        ownTurnState = new AngerOwnTurnState(this, controller, gameKnowledge);
        neutralState = new AngerNeutralState(this, controller, gameKnowledge);
        opponentTurnState = new AngerOpponentTurnState(this, controller, gameKnowledge);
        base.Reference(controller, gameKnowledge);
    }

    public ref readonly float MinDistanceToOpponent => ref minDistanceToOpponent;
}
