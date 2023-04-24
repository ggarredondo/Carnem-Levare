using UnityEngine;
using System;

public abstract class Controller : MonoBehaviour
{
    protected Vector2 movementVector;

    public ref readonly Vector2 MovementVector { get => ref movementVector; }
}
