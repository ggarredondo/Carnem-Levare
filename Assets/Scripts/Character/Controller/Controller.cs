using UnityEngine;
using System;

public abstract class Controller : MonoBehaviour
{
    protected Vector2 movementVector;
    public event Action OnDoBlock;

    public void Initialize()
    {
        movementVector = Vector2.zero;
    }

    protected void OnDoBlockInvoke() { OnDoBlock.Invoke(); }
    public ref readonly Vector2 MovementVector { get => ref movementVector; }
}
