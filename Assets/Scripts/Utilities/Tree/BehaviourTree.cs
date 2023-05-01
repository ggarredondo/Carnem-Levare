using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

[CreateAssetMenu(menuName = "Scriptable Objects/Tree/BehaviourTree")]
public class BehaviourTree : ScriptableObject
{
    public Node rootNode;
    public List<Node> nodes = new();
    [SerializeField] private Node currentNode;
    private readonly Stack<Node> nodeStack = new();
    [SerializeField] private int selectedDepth;
    public System.Action OnChange;

    public void Initialize()
    {
        selectedDepth = 1;
        currentNode = GetChildren((IHaveChildren)rootNode)[0];
        nodeStack.Clear();
        nodeStack.Push(currentNode);
        OnChange.Invoke();
    }

    public Node CreateNode(System.Type type)
    {
        Node node = CreateInstance(type) as Node;
        node.name = type.Name;
#if UNITY_EDITOR
        node.guid = GUID.Generate().ToString();
#endif
        nodes.Add(node);

#if UNITY_EDITOR
        AssetDatabase.AddObjectToAsset(node, this);
        AssetDatabase.SaveAssets();
#endif

        return node;
    }

    public void DeleteNode(Node node)
    {
        nodes.Remove(node);

#if UNITY_EDITOR
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();
#endif

    }

    public void AddChild(IHaveChildren parent, IHaveParent child)
    {
        parent.AddChild(child);
    }

    public void RemoveChild(IHaveChildren parent, IHaveParent child)
    {
        parent.RemoveChild(child);
    }

    public List<Node> GetChildren(IHaveChildren parent)
    {
        return parent.GetChildren();
    }

    public Node GetParent(IHaveParent child)
    {
        return child.GetParent();
    }

    public void GoToChild(int child = 0, int depth = 0)
    {
        if (currentNode is IHaveChildren node && node.HaveChildren()) 
        {
            currentNode = GetChildren(node)[child];
            nodeStack.Push(currentNode);

            if (currentNode is IHaveChildren newNode && newNode.Static())
            {
                selectedDepth++;
                GoToChild(child, depth + 1);
            }

            if(depth == 0) OnChange.Invoke();
        }
    }

    public void GoToParent(int depth = 0)
    {
        if (currentNode is IHaveParent node)
        {
            currentNode = GetParent(node);
            nodeStack.Pop();

            if (currentNode is IHaveChildren parent && parent.Static())
            {
                selectedDepth--;
                GoToParent(depth + 1);
            }

            if(depth == 0) OnChange.Invoke();
        }
    }

    private void GoToSelectableParent()
    {
        while (nodeStack.Count > 0 && nodeStack.Peek() is not ICanSelect)
        {
            if (nodeStack.Peek() is IHaveChildren parent && parent.Static())
                selectedDepth--;

            nodeStack.Pop();
        }

        currentNode = nodeStack.Peek();
    }

    public void MoveToRightSibling()
    {
        if (nodeStack.Any(n => n is ICanSelect))
        {
            GoToSelectableParent();

            if (currentNode is ICanSelect selectable)
            {
                selectable.MoveRightChild();
                GoToChild(selectable.GetSelectedChild());
            }
        }
    }

    public void MoveToLeftSibling()
    {
        if (nodeStack.Any(n => n is ICanSelect))
        {
            GoToSelectableParent();

            if (currentNode is ICanSelect selectable)
            {
                selectable.MoveLeftChild();
                GoToChild(selectable.GetSelectedChild());
            }
        }
    }

    public List<int> GetSelected()
    {
        return nodeStack.Take(selectedDepth).Select(node => node.id).ToList();
    }

    public int CurrentId()
    {
        return currentNode.id;
    }
}
