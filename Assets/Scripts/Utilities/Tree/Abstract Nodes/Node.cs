using UnityEngine;

public abstract class Node : ScriptableObject
{
    public int id;
    [System.NonSerialized] public bool selected;
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;

    public void Initialize()
    {
        selected = false;

        if (this is IHaveChildren n)
            n.InitializeChildren();
    }
}
