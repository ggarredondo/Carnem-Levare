using UnityEngine;
using System;
using RefDelegates;

public abstract class Controller : MonoBehaviour
{
    protected Vector2 movementVector;
    public bool isBlocking { get; private set; }
    public event Action OnDoBlock;
    public event Action<int> OnDoMove;
    public ActionIn<Hitbox> OnHurt;

    public virtual void Initialize()
    {
        movementVector = Vector2.zero;
    }

    protected void DoBlock(bool done) { isBlocking = done; OnDoBlock?.Invoke(); }
    protected void DoMove(int moveIndex) { OnDoMove?.Invoke(moveIndex); }
    public ref readonly Vector2 MovementVector { get => ref movementVector; }
}
