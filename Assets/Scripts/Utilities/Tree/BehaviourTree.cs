using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

[CreateAssetMenu(menuName = "Scriptable Objects/Tree/BehaviourTree")]
public class BehaviourTree : ScriptableObject
{
    public Node rootNode;
    public List<Node> nodes = new();
    private readonly Stack<Node> nodeStack = new();
    public System.Action OnChange;

    public void Initialize()
    {
        nodeStack.Clear();
        nodeStack.Push(GetChildren((IHaveChildren)rootNode)[0]);
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
        if (nodeStack.Peek() is IHaveChildren node && node.HaveChildren()) 
        {
            nodeStack.Push(GetChildren(node)[child]);

            if (nodeStack.Peek() is IHaveChildren newNode && newNode.Static()) 
            {
                if (newNode is ICanSelect selectable)
                    GoToChild(selectable.GetSelectedChild(), depth + 1);
                else
                    GoToChild(child, depth + 1);
            }

            if (depth == 0) OnChange.Invoke();
        }
    }

    public void GoToParent(int depth = 0)
    {
        if (nodeStack.Peek() is IHaveParent node && node.GetParent() is not RootNode)
        {
            nodeStack.Pop();

            if (nodeStack.Peek() is IHaveChildren parent && parent.Static())
                GoToParent(depth + 1);
            
            if(depth == 0) OnChange.Invoke();
        }
    }

    private void GoToSelectableParent()
    {
        while (nodeStack.Count > 0 && nodeStack.Peek() is not ICanSelect)
            nodeStack.Pop();
    }

    public bool ChangeSibling(int sibling)
    {
        if (nodeStack.Any(n => n is ICanSelect))
        {
            GoToSelectableParent();

            if (nodeStack.Peek() is ICanSelect selectable)
            {
                selectable.SelectChild(sibling);
                GoToChild(selectable.GetSelectedChild());
            }

            return true;
        }
        else return false;
    }

    public bool MoveToRightSibling()
    {
        if (nodeStack.Any(n => n is ICanSelect))
        {
            GoToSelectableParent();

            if (nodeStack.Peek() is ICanSelect selectable)
            {
                selectable.MoveRightChild();
                GoToChild(selectable.GetSelectedChild());
            }

            return true;
        }
        else return false;
    }

    public bool MoveToLeftSibling()
    {
        if (nodeStack.Any(n => n is ICanSelect))
        {
            GoToSelectableParent();

            if (nodeStack.Peek() is ICanSelect selectable)
            {
                selectable.MoveLeftChild();
                GoToChild(selectable.GetSelectedChild());
            }

            return true;
        }
        else return false;
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
        List<int> returnList = new();

        returnList.Add(nodeStack.Peek().id);

        for (int i = nodeStack.Count-1; i >= 0; i--)
        {
            if (nodeStack.ElementAt(i) is IHaveChildren newNode && newNode.Static())
                returnList.Add(nodeStack.ElementAt(i).id);
        }

        return returnList;
    }

    public int CurrentId()
    {
        return nodeStack.Peek().id;
    }
}
