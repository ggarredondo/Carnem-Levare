using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : Controller
{
    private CharacterStateMachine stateMachine;
    private Action<int> inputAction;
    private Action bufferedAction;

    private WaitForSeconds waitForSeconds;
    private IEnumerator couscous;
    [SerializeField] private double msBeforeBuffering = 300.0;
    private bool BUFFER_FLAG = false;

    public List<int> moveIndexes = new(4) { 0, 1, 2, 3 };
    [NonSerialized] public bool assigning;

    public void Reference(in CharacterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        inputAction = DoMove;
        waitForSeconds = new WaitForSeconds((float)TimeSpan.FromMilliseconds(msBeforeBuffering).TotalSeconds);
        
        stateMachine.MoveState.OnEnter += ClearBuffer;
        stateMachine.MoveState.OnEnter += () => { 
            inputAction = BufferMove;
            couscous = AllowBuffer();
            StartCoroutine(couscous); 
        };
        stateMachine.MoveState.OnExit += () => { 
            inputAction = DoMove; 
            StopCoroutine(couscous);
            BUFFER_FLAG = false;
        };
    }

    private void OnValidate() => waitForSeconds = new WaitForSeconds((float)TimeSpan.FromMilliseconds(msBeforeBuffering).TotalSeconds);

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
    }
    private IEnumerator AllowBuffer() 
    {
        yield return waitForSeconds;
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
}
