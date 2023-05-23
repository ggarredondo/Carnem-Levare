
public class AngerNeutralState : AIState
{
    private AngerAIStateMachine aiFSM;
    private AIController controller;
    private GameKnowledge gameKnowledge;
    public AngerNeutralState(in AngerAIStateMachine aiFSM, in AIController controller, in GameKnowledge gameKnowledge)
    {
        this.aiFSM = aiFSM;
        this.controller = controller;
        this.gameKnowledge = gameKnowledge;
    }

    public void Enter() {}
    public void Update() 
    {
        controller.Movement(0f, gameKnowledge.ImperfectDistance <= aiFSM.MinDistanceToOpponent ? 0f : 1f);
    }
    public void Exit() {}
}
