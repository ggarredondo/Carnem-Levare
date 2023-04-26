using System.Collections.Generic;

public abstract class CompositeNode : Node, IHaveChildren
{
    public List<Node> children = new();
    protected int actualChildren;

    public void AddChild(Node child)
    {
        children.Add(child);
    }

    public List<Node> GetChildren()
    {
        return children;
    }

    public void RemoveChild(Node child)
    {
        children.Remove(child);
    }

    public override Node Clone()
    {
        CompositeNode node = Instantiate(this);
        node.children = children.ConvertAll(c => c.Clone());
        return node;
    }
}
