using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour, IController
{
    private Vector2 movementVector;

    public void Movement(InputAction.CallbackContext context)
    {
        movementVector = context.ReadValue<Vector2>();
    }

    public Vector2 MovementVector { get => movementVector; }
}
