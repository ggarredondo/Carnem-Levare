using UnityEngine;

public class AIController : MonoBehaviour, IController
{
    private Vector2 movementVector;

    private void Awake()
    {
        movementVector = Vector2.zero;
    }

    public Vector2 MovementVector { get => movementVector; }
}
