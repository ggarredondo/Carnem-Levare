using UnityEngine;

public class Player : Character
{
    protected override void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Enemy").transform;
        base.Awake();
    }

    protected override void Start()
    {
        ((InputController)controller).Reference(stateMachine);

        if (DataSaver.games != null)
        {
            ((InputController)controller).moveIndexes = DataSaver.CurrentGameSlot.selectedMoves;
            characterStats.MoveList = DataSaver.CurrentGameSlot.moves;
        }

        base.Start();
    }
}
