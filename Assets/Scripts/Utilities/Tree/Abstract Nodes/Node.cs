using UnityEngine;

public abstract class Node : ScriptableObject
{
    public int id;
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;
}
