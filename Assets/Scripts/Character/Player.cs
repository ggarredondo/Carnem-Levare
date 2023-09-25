using UnityEngine;

public class Player : Character
{
    protected override void Start()
    {
        opponent = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();
        ((InputController)controller).Reference(stateMachine);

        if (GameManager.Save.IsLoaded())
        {
            ((InputController)controller).moveIndexes = DataSaver.Game.selectedMoves;
            characterStats.MoveList = DataSaver.Game.moves;
        }

        base.Start();
    }
}
