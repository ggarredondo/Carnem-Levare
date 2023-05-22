using UnityEngine;

public abstract class Node : ScriptableObject
{
    public int ID;
    public bool selected;
    public string guid;
    public Vector2 position;

    public ref readonly int GetID { get => ref ID; }

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
