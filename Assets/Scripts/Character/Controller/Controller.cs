using UnityEngine;
using System;

public abstract class Controller : MonoBehaviour
{
    protected Vector2 movementVector;
    public event Action OnDoBlock, OnStopBlock;

    public virtual void Initialize()
    {
        movementVector = Vector2.zero;
    }

    protected void OnDoBlockInvoke() { OnDoBlock?.Invoke(); }
    protected void OnStopBlockInvoke() { OnStopBlock?.Invoke(); }
    public ref readonly Vector2 MovementVector { get => ref movementVector; }
}
