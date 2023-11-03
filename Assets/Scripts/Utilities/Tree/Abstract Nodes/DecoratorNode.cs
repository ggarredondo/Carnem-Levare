using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TreeUtilities
{
    public class DecoratorNode : Node, IHaveChildren, IHaveParent
    {
        [HideInInspector] public Node child;
        [HideInInspector] public Node parent;

        public void AddChild(IHaveParent child)
        {
            child.SetParent(this);
            this.child = (Node)child;
        }

        public List<Node> GetChildren()
        {
            return new List<Node>() { child };
        }

        public Node GetParent()
        {
            return parent;
        }

        public bool HaveChildren()
        {
            return child != null;
        }

        public void InitializeChildren()
        {
            if (HaveChildren())
                child.Initialize();
        }

        public int InitializeChildrenID()
        {
            if (HaveChildren())
                child.InitializeID(ID);

            return ID;
        }

        public void RemoveChild(IHaveParent child)
        {
            this.child = null;
        }

        public void SetParent(Node parent)
        {
            this.parent = parent;
        }

        public bool Static()
        {
            return false;
        }
    }
}
