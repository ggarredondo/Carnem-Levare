using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour, IController
{
    private PlayerInput playerInput;
    private Vector2 movementVector;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Movement"].performed += Movement;
    }

    private void Movement(InputAction.CallbackContext context)
    {
        movementVector = context.ReadValue<Vector2>();
    }

    public Vector2 MovementVector { get => movementVector; }
}
