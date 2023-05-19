using UnityEngine;

public abstract class Node : ScriptableObject
{
    public int ID;
    [System.NonSerialized] public bool selected;
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;

    public int InitializeID(int actualID)
    {
        ID = actualID + 1;
        int newID = ID;

        if (this is IHaveChildren n)
            newID = n.InitializeChildrenID();

        return newID;
    }

    public void Initialize()
    {
        selected = false;

        if (this is IHaveChildren n)
            n.InitializeChildren();
    }
}
