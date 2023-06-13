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
        ((InputController)controller).moveIndexes = DataSaver.games[DataSaver.currentGameSlot].selectedMoves;
        characterStats.MoveList = DataSaver.games[DataSaver.currentGameSlot].moves;
        base.Start();
    }
}
