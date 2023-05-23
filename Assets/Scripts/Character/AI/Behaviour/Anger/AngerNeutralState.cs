
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
        if (gameKnowledge.ImperfectDistance > aiFSM.MinDistanceToOpponent)
            controller.Movement(0f, 1f);
        else
        {
            controller.Movement(0f, 0f);
            //aiFSM.TransitionToOwnTurn();
        }    
    }
    public void Exit() {}
}
