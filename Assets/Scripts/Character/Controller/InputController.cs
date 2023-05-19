using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : Controller
{
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
        if (context.performed) DoMove(0);
    }
    public void Action1(InputAction.CallbackContext context)
    {
        if (context.performed) DoMove(1);
    }
    public void Action2(InputAction.CallbackContext context)
    {
        if (context.performed) DoMove(2);
    }
    public void Action3(InputAction.CallbackContext context)
    {
        if (context.performed) DoMove(3);
    }
}
