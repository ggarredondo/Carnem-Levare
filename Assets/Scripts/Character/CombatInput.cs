using UnityEngine;
using UnityEngine.InputSystem;

public class CombatInput : Singleton<CombatInput>
{
    public Vector2 movementVector { get; private set; }

    public void Movement(InputAction.CallbackContext context)
    {
        movementVector = context.ReadValue<Vector2>();
    }
}
