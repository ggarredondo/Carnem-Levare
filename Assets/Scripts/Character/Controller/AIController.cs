using UnityEngine;

public class AIController : Controller
{
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
}
