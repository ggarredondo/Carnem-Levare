using System.Collections.Generic;
using UnityEngine;

public class MoveMenu : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private List<Move> moves;
    [SerializeField] private MoveSelector moveSelector;
    [SerializeField] private MoveInfo infoBox;
    [SerializeField] private InputReader inputReader;

    private void Awake()
    {
        moveSelector.Initialize(ref moves);
        infoBox.Initialize();
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
            moveSelector.LeftMoveBlock();
        if (value == new Vector2(1, 0))
            moveSelector.RightMoveBlock();

        Move actualMove = moves[moveSelector.GetActualIndex()];
        infoBox.UpdateInfoBox(ref actualMove);
    }
}
