using System.Collections.Generic;

public interface IHaveChildren
{
    public void AddChild(Node child);
    public void RemoveChild(Node child);
    public List<Node> GetChildren();
}
