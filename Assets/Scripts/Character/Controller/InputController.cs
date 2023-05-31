using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : Controller
{
    [ReadOnlyField] public List<int> ACTION_INDEX = new(4) { 0, 1, 2, 3 };
    [ReadOnlyField] public bool assigning;

    public void PressMovement(InputAction.CallbackContext context)
    {
        movementVector = context.ReadValue<Vector2>();
    }
    public void HoldBlock(InputAction.CallbackContext context)
    {
        DoBlock(context.performed);
    }

    public void Action0(InputAction.CallbackContext context)
    {
        if (context.performed && !assigning) DoMove(ACTION_INDEX[0]);
    }
    public void Action1(InputAction.CallbackContext context)
    {
        if (context.performed && !assigning) DoMove(ACTION_INDEX[1]);
    }
    public void Action2(InputAction.CallbackContext context)
    {
        if (context.performed && !assigning) DoMove(ACTION_INDEX[2]);
    }
    public void Action3(InputAction.CallbackContext context)
    {
        if (context.performed && !assigning) DoMove(ACTION_INDEX[3]);
    }
}
