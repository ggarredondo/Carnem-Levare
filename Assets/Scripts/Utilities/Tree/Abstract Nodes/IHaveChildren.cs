using System.Collections.Generic;

namespace TreeUtilities
{
    public interface IHaveChildren
    {
        public void AddChild(IHaveParent child);
        public void RemoveChild(IHaveParent child);
        public List<Node> GetChildren();

        public bool HaveChildren();

        public int InitializeChildrenID();

        public void InitializeChildren();

        public bool Static();
    }
}
