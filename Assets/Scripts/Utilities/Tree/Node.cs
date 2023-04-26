using UnityEngine;

public abstract class Node : ScriptableObject
{
    public bool selected;
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;

    public void ChangeState()
    {
        if (selected) OnNotSelected();
        else OnSelected();
    }

    public virtual Node Clone()
    {
        return Instantiate(this);
    }

    protected abstract void OnNotSelected();
    protected abstract void OnSelected();
}
