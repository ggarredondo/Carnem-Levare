using System.Collections.Generic;
using UnityEngine;

public class RootNode : Node, IHaveChildren
{
    [HideInInspector] public Node child;

    public void AddChild(IHaveParent child)
    {
        child.SetParent(this);
        this.child = (Node) child;
    }

    public List<Node> GetChildren()
    {
        return new List<Node>() { child };
    }

    public void RemoveChild(IHaveParent Child)
    {
        child = null;
    }

    public bool HaveChildren()
    {
        return child != null;
    }

    public bool Static()
    {
        return false;
    }
}
