using System.Collections.Generic;
using UnityEngine;

public class MoveMenu : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private MoveSelector moveSelector;
    [SerializeField] private MoveInfo infoBox;
    [SerializeField] private InputReader inputReader;

    private List<Move> moves;
    private InputController inputController;

    private void Start()
    {
        moves = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CharacterStats.MoveList;
        inputController = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Controller as InputController;

        moveSelector.Initialize(ref moves, ref inputController);
        infoBox.Initialize(in moves[moveSelector.GetActualIndex()].StringData);

        GameManager.Input.ControlsChangedEvent += moveSelector.ChangeInputMap;
    }

    private void OnEnable()
    {
        inputReader.DPAdEvent += DPad;
    }

    private void OnDisable()
    {
        inputReader.DPAdEvent -= DPad;
    }

    private void OnDestroy()
    {
        GameManager.Input.ControlsChangedEvent -= moveSelector.ChangeInputMap;
    }

    private void DPad(Vector2 value)
    {
        if (value == new Vector2(-1, 0))
        {
            if (moveSelector.LeftMoveBlock())
            {
                infoBox.UpdateInfoBox(in moves[moveSelector.GetActualIndex()].StringData);
                GameManager.Audio.Play("InteractMoveMenu");
            }
            else
            {
                infoBox.Movement(new Color(0.5f, 0.5f, 0.5f));
                GameManager.Audio.Play("NotInteractMoveMenu");
            }
        }
        if (value == new Vector2(1, 0))
        {
            if (moveSelector.RightMoveBlock())
            {
                infoBox.UpdateInfoBox(in moves[moveSelector.GetActualIndex()].StringData);
                GameManager.Audio.Play("InteractMoveMenu");
            }
            else
            {
                infoBox.Movement(new Color(0.5f, 0.5f, 0.5f));
                GameManager.Audio.Play("NotInteractMoveMenu");
            }
        }

        if (value == new Vector2(0, 1))
        {
            moveSelector.SelectBlock();
            GameManager.Audio.Play("SelectMoveMenu");
        }

        if (value == new Vector2(0, -1))
        {
            moveSelector.DeselectBlock();
            GameManager.Audio.Play("InteractMoveMenu");
        }
    }
}
