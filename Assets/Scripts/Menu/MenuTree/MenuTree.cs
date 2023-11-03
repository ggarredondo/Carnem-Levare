using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TreeUtilities;

namespace MenuTreeUtilities
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Tree/MenuTree")]
    public class MenuTree : BehaviourTree
    {
        private readonly Stack<Node> nodeStack = new();
        public System.Action OnChange;

        public void Initialize()
        {
            nodeStack.Clear();
            nodeStack.Push(rootNode);
            GoToChild();
            OnChange.Invoke();
        }

        public void GoToChild(int child = 0, int depth = 0)
        {
            if (nodeStack.Peek() is IHaveChildren node && node.HaveChildren())
            {
                if (node is ICanSelect selectable)
                    nodeStack.Push(node.GetChildren()[selectable.GetSelectedChild()]);
                else
                    nodeStack.Push(node.GetChildren()[child]);

                if (nodeStack.Peek() is IHaveChildren newNode && newNode.Static())
                    GoToChild(child, depth + 1);

                if (depth == 0) OnChange.Invoke();
            }
        }

        public bool GoToParent()
        {
            if (CanGoToParent())
            {
                while (true)
                {
                    nodeStack.Pop();

                    if (nodeStack.Peek().selected) continue;
                    else break;
                }

                OnChange.Invoke();

                return true;
            }
            else return false;
        }

        private bool CanGoToParent()
        {
            for (int i = 1; i < nodeStack.Count; i++)
                if (nodeStack.ElementAt(i) is not RootNode && !nodeStack.ElementAt(i).selected)
                    return true;

            return false;
        }

        private void GoToSelectableParent()
        {
            while (nodeStack.Peek() is not ICanSelect && nodeStack.ElementAt(1).selected)
                nodeStack.Pop();
        }

        public bool ChangeSibling(int sibling)
        {
            bool result = false;

            if (nodeStack.Any(n => n is ICanSelect))
            {
                GoToSelectableParent();

                if (nodeStack.Peek() is ICanSelect selectable)
                {
                    selectable.SelectChild(sibling);
                    GoToChild();
                    result = true;
                }
            }

            return result;
        }

        public bool MoveToRightSibling()
        {
            bool result = false;

            if (nodeStack.Any(n => n is ICanSelect))
            {
                GoToSelectableParent();

                if (nodeStack.Peek() is ICanSelect selectable)
                {
                    selectable.MoveRightChild();
                    GoToChild();
                    result = true;
                }
            }

            return result;
        }

        public bool MoveToLeftSibling()
        {
            bool result = false;

            if (nodeStack.Any(n => n is ICanSelect))
            {
                GoToSelectableParent();

                if (nodeStack.Peek() is ICanSelect selectable)
                {
                    selectable.MoveLeftChild();
                    GoToChild();
                    result = true;
                }
            }

            return result;
        }

        public int ActualSelectableID()
        {
            if (nodeStack.Any(n => n is ICanSelect))
                return ((ICanSelect)nodeStack.FirstOrDefault(n => n is ICanSelect)).GetSelectedChild();
            else
                return 0;
        }

        public List<int> GetSelected()
        {
            rootNode.Initialize();

            List<int> returnList = new();

            returnList.Add(nodeStack.Peek().ID);
            nodeStack.Peek().selected = true;

            for (int i = 1; i < nodeStack.Count; i++)
            {
                if (nodeStack.ElementAt(i) is IHaveChildren newNode)
                {
                    if (newNode.Static())
                    {
                        returnList.Add(nodeStack.ElementAt(i).ID);
                        nodeStack.ElementAt(i).selected = true;
                    }
                    else break;
                }
            }

            return returnList;
        }

        public int CurrentId()
        {
            return nodeStack.Peek().ID;
        }
    }
}
