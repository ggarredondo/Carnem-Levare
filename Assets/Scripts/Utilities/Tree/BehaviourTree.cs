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
    private readonly Stack<Node> pathNodes = new();
    [SerializeField] private int selectedDepth;
    public System.Action OnChange;

    public void Initialize()
    {
        selectedDepth = 1;
        currentNode = GetChildren((IHaveChildren)rootNode)[0];
        pathNodes.Clear();
        pathNodes.Push(currentNode);
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
            pathNodes.Push(currentNode);

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
            pathNodes.Pop();

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
        while (pathNodes.Count > 0 && pathNodes.Peek() is not ICanSelect)
        {
            pathNodes.Pop();

            if (pathNodes.Peek() is IHaveChildren parent && parent.Static())
                selectedDepth--;
        }

        currentNode = pathNodes.Peek();
    }

    public List<int> GetSelected()
    {
        return pathNodes.Take(selectedDepth).Select(node => node.id).ToList();
    }
}
