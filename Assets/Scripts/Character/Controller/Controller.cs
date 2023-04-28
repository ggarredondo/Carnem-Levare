using UnityEngine;
using System;

public abstract class Controller : MonoBehaviour
{
    protected Vector2 movementVector;
    public bool isBlocking { get; private set; }
    public event Action<bool> OnDoBlock;
    public event Action<int> OnDoMove;

    public virtual void Initialize()
    {
        movementVector = Vector2.zero;
    }

    protected void OnDoBlockInvoke(bool done) { isBlocking = done; OnDoBlock?.Invoke(done); }
    protected void OnDoMoveInvoke(int moveIndex) { OnDoMove?.Invoke(moveIndex); }
    public ref readonly Vector2 MovementVector { get => ref movementVector; }
}
