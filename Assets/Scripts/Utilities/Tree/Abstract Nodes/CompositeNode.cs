using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeNode : Node, IHaveChildren, IHaveParent
{
    public bool isStatic;
    [HideInInspector] public List<Node> children = new();
    [HideInInspector] public Node parent;

    public void AddChild(IHaveParent child)
    {
        child.SetParent(this);
        children.Add((Node) child);
    }

    public List<Node> GetChildren()
    {
        return children;
    }

    public void RemoveChild(IHaveParent child)
    {
        children.Remove((Node) child);
    }

    public void SetParent(Node parent)
    {
        this.parent = parent;
    }

    public Node GetParent()
    {
        return parent;
    }

    public bool HaveChildren()
    {
        return children.Count > 0;
    }

    public bool Static()
    {
        return isStatic;
    }
}
