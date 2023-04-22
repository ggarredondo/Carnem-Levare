using UnityEngine;

public class AIController : MonoBehaviour, IController
{
    private Vector2 movementVector;
    [SerializeField] [Range(-1f, 1f)] private float horizontal, vertical;

    private void Awake()
    {
        movementVector = Vector2.zero;
    }

    private void OnValidate()
    {
        movementVector.x = horizontal;
        movementVector.y = vertical;
    }

    public ref readonly Vector2 MovementVector { get => ref movementVector; }
}
