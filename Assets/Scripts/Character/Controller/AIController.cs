using UnityEngine;

public class AIController : Controller
{
    [SerializeField] [Range(-1f, 1f)] private float horizontal, vertical;
    [SerializeField] private bool block = false;

    private void OnValidate()
    {
        movementVector.x = horizontal;
        movementVector.y = vertical;
        OnDoBlockInvoke(block);
    }
}
