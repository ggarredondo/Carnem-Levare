using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : Controller
{
    [ReadOnlyField] public int ACTION0_INDEX = 0, ACTION1_INDEX = 1, ACTION2_INDEX = 2, ACTION3_INDEX = 3;

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
        if (context.performed) DoMove(ACTION0_INDEX);
    }
    public void Action1(InputAction.CallbackContext context)
    {
        if (context.performed) DoMove(ACTION1_INDEX);
    }
    public void Action2(InputAction.CallbackContext context)
    {
        if (context.performed) DoMove(ACTION2_INDEX);
    }
    public void Action3(InputAction.CallbackContext context)
    {
        if (context.performed) DoMove(ACTION3_INDEX);
    }
}
