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
        OnDoBlockInvoke();
    }
}
