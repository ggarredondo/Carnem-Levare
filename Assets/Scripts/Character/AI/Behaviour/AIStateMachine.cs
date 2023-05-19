using UnityEngine;

public abstract class AIStateMachine : ScriptableObject
{
    protected AIState currentState, ownTurnState, neutralState, opponentTurnState;

    public virtual void Reference(in AIController controller, in GameKnowledge gameKnowledge) => currentState = neutralState;

    private void ChangeState(in AIState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Enable(bool enabled) {}

    public void TransitionToOwnTurn() => ChangeState(ownTurnState);
    public void TransitionToNeutral() => ChangeState(neutralState);
    public void TransitionToOpponentTurn() => ChangeState(opponentTurnState);
    public ref readonly AIState CurrentState => ref currentState;
}
