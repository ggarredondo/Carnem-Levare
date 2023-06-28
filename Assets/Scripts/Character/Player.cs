using UnityEngine;

public class Player : Character
{
    protected override void Start()
    {
        opponent = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();
        ((InputController)controller).Reference(stateMachine);

        if (DataSaver.games != null)
        {
            ((InputController)controller).moveIndexes = DataSaver.CurrentGameSlot.selectedMoves;
            characterStats.MoveList = DataSaver.CurrentGameSlot.moves;
        }

        base.Start();
    }
}
