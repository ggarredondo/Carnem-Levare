using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MoveAssignment : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;

    private readonly List<string> inputAssignments = new(4);

    private InputController inputController;
    private int newMove;

    private void OnEnable()
    {
        inputReader.Action0 += ReadInput;
        inputReader.Action1 += ReadInput;
        inputReader.Action2 += ReadInput;
        inputReader.Action3 += ReadInput;
    }

    private void OnDisable()
    {
        inputReader.Action0 += ReadInput;
        inputReader.Action1 += ReadInput;
        inputReader.Action2 += ReadInput;
        inputReader.Action3 += ReadInput;
    }

    public void Initialize(ref List<RectTransform> moves, ref InputController inputController)
    {
        this.inputController = inputController;
        UpdateInput(ref moves);
    }

    public void UpdateInput(ref List<RectTransform> moves)
    {
        inputAssignments.Clear();
        inputAssignments.Add(GameManager.InputMapping.ObtainMapping("Action0"));
        inputAssignments.Add(GameManager.InputMapping.ObtainMapping("Action1"));
        inputAssignments.Add(GameManager.InputMapping.ObtainMapping("Action2"));
        inputAssignments.Add(GameManager.InputMapping.ObtainMapping("Action3"));

        moves.ForEach(m => m.GetComponent<MoveBlock>().Disable());

        for(int i = 0; i < inputController.ACTION_INDEX.Count; i++)
        {
            moves[inputController.ACTION_INDEX[i]].GetComponent<MoveBlock>().AsignInput(inputAssignments[i]);
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
            inputController.ACTION_INDEX[newMove] = moveSelected;
            Debug.Log(moveSelected);
            inputController.assigning = false;
        }
    }

    public void EndChangeInput()
    {
        inputController.assigning = false;
    }
}
