using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Tree/DialogueTree")]
public class DialogueTree : BehaviourTree
{
    private readonly Stack<Node> nodeStack = new();
    private IHaveText currentNode;

    public void Initialize()
    {
        nodeStack.Clear();
        nodeStack.Push(rootNode);
        Next();
    }

    private void UpdateCurrentNode()
    {
        currentNode = (IHaveText)nodeStack.Peek();
        nodeStack.Peek().selected = true;
    }

    public IHaveText CurrentLine => currentNode;

    public void Next()
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
        }
    }

    public void Previous()
    {
        if (nodeStack.Peek() is IHaveParent node && node.GetParent() is not RootNode)
        {
            nodeStack.Peek().selected = false;
            nodeStack.Push(node.GetParent());
            UpdateCurrentNode();
        }
    }
}
