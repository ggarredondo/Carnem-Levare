
public class AngerNeutralState : AIState
{
    private AIStateMachine aiFSM;
    private AIController controller;
    private GameKnowledge gameKnowledge;
    public AngerNeutralState(in AIStateMachine aiFSM, in AIController controller, in GameKnowledge gameKnowledge)
    {
        this.aiFSM = aiFSM;
        this.controller = controller;
        this.gameKnowledge = gameKnowledge;
    }

    public void Enter() {}
    public void Update() 
    {
        controller.Movement(0f, gameKnowledge.Distance <= 3f ? 0f : 1f);
    }
    public void Exit() {}
}
