using UnityEngine;

public abstract class AIStateMachine : ScriptableObject
{
    protected AIState currentState, ownTurnState, neutralState, opponentTurnState, emptyState;

    public virtual void Reference(in AIController controller, in GameKnowledge gameKnowledge)
    {
        emptyState = new EmptyState();
        ChangeState(neutralState);
    }

    private void ChangeState(in AIState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Enable(bool enabled)
    {
        if (enabled && neutralState != null) TransitionToNeutral();
        else if (emptyState != null) TransitionToEmpty();
    }

    public void TransitionToOwnTurn() => ChangeState(ownTurnState);
    public void TransitionToNeutral() => ChangeState(neutralState);
    public void TransitionToOpponentTurn() => ChangeState(opponentTurnState);
    public void TransitionToEmpty() => ChangeState(emptyState);
    public ref readonly AIState CurrentState => ref currentState;
}
