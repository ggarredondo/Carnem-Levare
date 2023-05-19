
public class AIStateMachine
{
    private readonly AIController controller;
    private readonly GameKnowledge gameKnowledge;

    private AIState currentState, neutralState;

    public AIStateMachine(in AIController controller, in GameKnowledge gameKnowledge)
    {
        this.controller = controller;
        this.gameKnowledge = gameKnowledge;

        neutralState = new AngerNeutralState(this, controller, gameKnowledge);
        currentState = neutralState;
    }

    private void ChangeState(in AIState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Enable(bool enabled) {}

    public ref readonly AIState CurrentState => ref currentState;
}
