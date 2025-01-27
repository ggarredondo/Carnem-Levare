using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MoveAssignment : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    private readonly List<string> inputAssignments = new(5);

    private InputController inputController;
    private int newMove;

    private void OnEnable()
    {
        inputReader.Action0 += ReadInput;
        inputReader.Action1 += ReadInput;
        inputReader.Action2 += ReadInput;
        inputReader.Action3 += ReadInput;
        inputReader.Action4 += ReadInput;
    }

    private void OnDisable()
    {
        inputReader.Action0 -= ReadInput;
        inputReader.Action1 -= ReadInput;
        inputReader.Action2 -= ReadInput;
        inputReader.Action3 -= ReadInput;
        inputReader.Action4 -= ReadInput;
    }

    public void Initialize(ref List<MoveBlock> moves, ref InputController inputController)
    {
        this.inputController = inputController;
        InitializeNewMoves(ref moves);
        UpdateInput(ref moves);
    }

    private void InitializeNewMoves(ref List<MoveBlock> moves)
    {
        for (int i = 0; i < moves.Count; i++)
        {
            moves[i].SetNewMove(i);
        }
    }

    public void UpdateInput(ref List<MoveBlock> moves)
    {
        inputAssignments.Clear();
        inputAssignments.Add(GameManager.Input.ObtainAllowedMapping("Action0"));
        inputAssignments.Add(GameManager.Input.ObtainAllowedMapping("Action1"));
        inputAssignments.Add(GameManager.Input.ObtainAllowedMapping("Action2"));
        inputAssignments.Add(GameManager.Input.ObtainAllowedMapping("Action3"));
        inputAssignments.Add(GameManager.Input.ObtainAllowedMapping("Action4"));

        moves.ForEach(m => m.GetComponent<MoveBlock>().Disable());

        for(int i = 0; i < inputController.moveIndexes.Count; i++)
        {
            if(GameManager.Save.IsLoaded())
                DataSaver.Game.selectedMoves[i] = inputController.moveIndexes[i];

            if(inputController.moveIndexes[i] < moves.Count)
                moves[inputController.moveIndexes[i]].GetComponent<MoveBlock>().AsignInput(inputAssignments[i]);
        }
    }

    private void ReadInput(int input)
    {
        newMove = input;
    }

    private async Task ChangeInput()
    {
        newMove = -1;

        while(newMove == -1 && inputController.assigning)
        {
            await Task.Yield();
        }
    }

    public async Task StartChangeInput(int moveSelected)
    {
        inputController.assigning = true;

        await ChangeInput();

        if (inputController.assigning)
        {
            GameManager.Audio.Play("ApplyMoveMenu");
            inputController.moveIndexes[newMove] = moveSelected;
        }
    }

    public void EndChangeInput()
    {
        inputController.assigning = false;
    }
}
