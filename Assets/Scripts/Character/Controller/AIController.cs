using UnityEngine;

public class AIController : Controller
{
    [SerializeField] [Range(-1f, 1f)] private float horizontal, vertical;
    [SerializeField] private bool block = false;
    [SerializeField] private bool move0, move1, move2, move3;

    private void OnValidate()
    {
        movementVector.x = horizontal;
        movementVector.y = vertical;
        DoBlock(block);

        if (move0) { move0 = false; DoMove(0); }
        if (move1) { move1 = false; DoMove(1); }
        if (move2) { move2 = false; DoMove(2); }
        if (move3) { move3 = false; DoMove(3); }
    }
}
