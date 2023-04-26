using System.Collections.Generic;

public class RootNode : Node, IHaveChildren
{
    public Node child;

    public override Node Clone()
    {
        RootNode node = Instantiate(this);
        node.child = child.Clone();
        return node;
    }

    public void AddChild(Node child)
    {
        this.child = child;
    }

    public List<Node> GetChildren()
    {
        return new List<Node>() { child };
    }

    public void RemoveChild(Node Child)
    {
        child = null;
    }

    protected override void OnNotSelected()
    {
        child.ChangeState();
    }

    protected override void OnSelected()
    {
        child.ChangeState();
    }
}
