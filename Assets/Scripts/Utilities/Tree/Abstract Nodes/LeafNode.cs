using UnityEngine;

namespace TreeUtilities
{
    public abstract class LeafNode : Node, IHaveParent
    {
        [HideInInspector] public Node parent;

        public Node GetParent()
        {
            return parent;
        }

        public void SetParent(Node parent)
        {
            this.parent = parent;
        }
    }
}
