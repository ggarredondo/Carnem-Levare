using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovePair
{
    [SerializeField] private int moveIndex;
    [SerializeField] private double msUntilNextMove;
    public int MoveIndex => moveIndex;
    public double MsUntilNextMove => msUntilNextMove;
}

[System.Serializable]
public class MoveSequence
{
    [SerializeField] private List<MovePair> sequence;
    public MovePair this[int index] => sequence[index];
}
