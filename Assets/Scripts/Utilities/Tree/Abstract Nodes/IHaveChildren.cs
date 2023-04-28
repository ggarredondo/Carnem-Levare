using System.Collections.Generic;

public interface IHaveChildren
{
    public void AddChild(IHaveParent child);
    public void RemoveChild(IHaveParent child);
    public List<Node> GetChildren();

    public bool HaveChildren();

    public bool Static();
}
