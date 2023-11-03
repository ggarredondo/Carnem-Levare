using System.Collections.Generic;
using UnityEngine;

namespace TreeUtilities
{
    public abstract class CompositeNode : Node, IHaveChildren, IHaveParent
    {
        public bool isStatic;
        [HideInInspector] public List<Node> children = new();
        [HideInInspector] public Node parent;

        public void AddChild(IHaveParent child)
        {
            child.SetParent(this);
            children.Add((Node)child);
        }

        public List<Node> GetChildren()
        {
            return children;
        }

        public void RemoveChild(IHaveParent child)
        {
            children.Remove((Node)child);
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

        public void InitializeChildren()
        {
            if (HaveChildren())
                children.ForEach(c => c.Initialize());
        }

        public int InitializeChildrenID()
        {
            int actualID = ID;

            if (HaveChildren())
                children.ForEach(c => actualID = c.InitializeID(actualID));

            return actualID;
        }
    }
}
