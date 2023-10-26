using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : Controller
{
    private CharacterStateMachine stateMachine;
    private Action<int> inputAction;
    private Action bufferedAction;
    private bool BUFFER_FLAG;

    public List<int> moveIndexes = new(5) { 0, 1, 2, 3, 4 };
    [NonSerialized] public bool assigning;

    public void Reference(in CharacterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        inputAction = DoMove;

        stateMachine.MoveState.OnEnter += ClearBuffer;
        stateMachine.OnDeactivateMove += () => inputAction = BufferMove;

        stateMachine.WalkingState.OnEnter += () => inputAction = DoMove;
        stateMachine.BlockingState.OnEnter += () => inputAction = DoMove;
    }

    private void BufferMove(int moveIndex)
    {
        if (BUFFER_FLAG) {
            BUFFER_FLAG = false;
            bufferedAction = () => DoMove(moveIndex);
            stateMachine.WalkingState.OnEnter += bufferedAction;
            stateMachine.BlockingState.OnEnter += bufferedAction;
        }
    }
    private void ClearBuffer()
    {
        stateMachine.WalkingState.OnEnter -= bufferedAction;
        stateMachine.BlockingState.OnEnter -= bufferedAction;
        BUFFER_FLAG = true;
    }

    public void PressMovement(InputAction.CallbackContext context)
    {
        movementVector = context.ReadValue<Vector2>();
    }
    public void HoldBlock(InputAction.CallbackContext context)
    {
        if (!context.started) DoBlock(context.performed);
    }
    public void Action0(InputAction.CallbackContext context)
    {
        if (context.performed && !assigning) inputAction.Invoke(moveIndexes[0]);
    }
    public void Action1(InputAction.CallbackContext context)
    {
        if (context.performed && !assigning) inputAction.Invoke(moveIndexes[1]);
    }
    public void Action2(InputAction.CallbackContext context)
    {
        if (context.performed && !assigning) inputAction.Invoke(moveIndexes[2]);
    }
    public void Action3(InputAction.CallbackContext context)
    {
        if (context.performed && !assigning) inputAction.Invoke(moveIndexes[3]);
    }
    public void Action4(InputAction.CallbackContext context)
    {
        if (context.performed && !assigning) inputAction.Invoke(moveIndexes[4]);
    }
}
