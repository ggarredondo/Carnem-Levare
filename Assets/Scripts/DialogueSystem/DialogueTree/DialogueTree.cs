using System.Collections.Generic;
using TreeUtilities;
using UnityEngine;

namespace DialogueTreeUtilities
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Tree/DialogueTree")]
    public class DialogueTree : BehaviourTree
    {
        private readonly Stack<Node> nodeStack = new();
        private IHaveText currentNode;

        public void Initialize()
        {
            nodeStack.Clear();
            nodeStack.Push(rootNode);
        }

        private void UpdateCurrentNode()
        {
            currentNode = (IHaveText)nodeStack.Peek();
            nodeStack.Peek().selected = true;
        }

        public DialogueData CurrentData => currentNode.Data;

        public string CurrentText => currentNode.Text;

        public bool Next()
        {
            if (nodeStack.Peek() is IHaveChildren node && node.HaveChildren())
            {
                List<Node> children = node.GetChildren();

                if (children.Count == 1)
                {
                    nodeStack.Peek().selected = false;
                    nodeStack.Push(children[0]);
                    UpdateCurrentNode();
                }
                return true;
            }
            else return false;
        }

        public bool Previous()
        {
            if (nodeStack.Peek() is IHaveParent node && node.GetParent() is not RootNode)
            {
                nodeStack.Peek().selected = false;
                nodeStack.Push(node.GetParent());
                UpdateCurrentNode();

                return true;
            }
            else return false;
        }
    }
}
