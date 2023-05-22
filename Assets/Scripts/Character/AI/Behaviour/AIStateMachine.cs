using UnityEngine;

public abstract class AIStateMachine : ScriptableObject
{
    protected AIState currentState, ownTurnState, neutralState, opponentTurnState, debugState;

    public virtual void Reference(in AIController controller, in GameKnowledge gameKnowledge)
    {
        debugState = new DebugState();
        currentState = neutralState;
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
        else if (debugState != null) TransitionToDebug();
    }

    public void TransitionToOwnTurn() => ChangeState(ownTurnState);
    public void TransitionToNeutral() => ChangeState(neutralState);
    public void TransitionToOpponentTurn() => ChangeState(opponentTurnState);
    public void TransitionToDebug() => ChangeState(debugState);
    public ref readonly AIState CurrentState => ref currentState;
}
