using System.Collections.Generic;
using UnityEngine;

public class MoveMenu : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private MoveSelector moveSelector;
    [SerializeField] private MoveInfo infoBox;
    [SerializeField] private InputReader inputReader;

    private List<Move> moves;

    private void Awake()
    {
        moves = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CharacterStats.MoveList;
        moveSelector.Initialize(ref moves);
        infoBox.Initialize(in moves[moveSelector.GetActualIndex()].StringData);
    }

    private void OnEnable()
    {
        inputReader.DPAdEvent += DPad;
    }

    private void OnDisable()
    {
        inputReader.DPAdEvent -= DPad;
    }

    private void DPad(Vector2 value)
    {
        if (value == new Vector2(-1, 0))
        {
            if (moveSelector.LeftMoveBlock())
                infoBox.UpdateInfoBox(in moves[moveSelector.GetActualIndex()].StringData);
            else infoBox.Movement(new Color(0.5f,0.5f,0.5f));
        }
        if (value == new Vector2(1, 0))
        {
            if(moveSelector.RightMoveBlock())
                infoBox.UpdateInfoBox(in moves[moveSelector.GetActualIndex()].StringData);
            else infoBox.Movement(new Color(0.5f, 0.5f, 0.5f));
        }

        if (value == new Vector2(0, 1))
        {
            moveSelector.SelectBlock();
        }

        if (value == new Vector2(0, -1))
        {
            moveSelector.DeselectBlock();
        }
    }
}
