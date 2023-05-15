using UnityEngine;

public abstract class Node : ScriptableObject
{
    public int id;
    [System.NonSerialized] public bool selected;
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;
}
