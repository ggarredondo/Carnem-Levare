using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : Controller
{
    public void DoMovement(InputAction.CallbackContext context)
    {
        movementVector = context.ReadValue<Vector2>();
    }
    public void DoBlock(InputAction.CallbackContext context)
    {
        if (context.performed) OnDoBlockInvoke();
        else OnStopBlockInvoke();
    }

    public void Action0(InputAction.CallbackContext context)
    {
        if (context.performed) OnDoMoveInvoke(0);
    }
    public void Action1(InputAction.CallbackContext context)
    {
        if (context.performed) OnDoMoveInvoke(1);
    }
    public void Action2(InputAction.CallbackContext context)
    {
        if (context.performed) OnDoMoveInvoke(2);
    }
    public void Action3(InputAction.CallbackContext context)
    {
        if (context.performed) OnDoMoveInvoke(3);
    }
}
