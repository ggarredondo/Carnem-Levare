using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class BehaviourTree : ScriptableObject
{
    public Node rootNode;
    public List<Node> nodes = new();

    public Node CreateNode(System.Type type)
    {
        Node node = CreateInstance(type) as Node;
        node.name = type.Name;
#if UNITY_EDITOR
        node.guid = GUID.Generate().ToString();
        Undo.RecordObject(this, "Behaviour Tree (CreateNode)");
#endif
        nodes.Add(node);

#if UNITY_EDITOR
        AssetDatabase.AddObjectToAsset(node, this);
        Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (CreateNode)");
        AssetDatabase.SaveAssets();
#endif

        return node;
    }

    public void DeleteNode(Node node)
    {
#if UNITY_EDITOR
        Undo.RecordObject(this, "Behaviour Tree (DeleteNode)");
#endif
        nodes.Remove(node);

#if UNITY_EDITOR
        Undo.DestroyObjectImmediate(node);
        AssetDatabase.SaveAssets();
#endif

    }

    public void AddChild(IHaveChildren parent, IHaveParent child)
    {
#if UNITY_EDITOR
        Undo.RecordObject((Node)parent, "Behaviour Tree (AddChild)");
#endif
        parent.AddChild(child);
#if UNITY_EDITOR
        EditorUtility.SetDirty((Node)parent);
#endif
    }

    public void RemoveChild(IHaveChildren parent, IHaveParent child)
    {
#if UNITY_EDITOR
        Undo.RecordObject((Node)parent, "Behaviour Tree (RemoveChild)");
#endif
        parent.RemoveChild(child);
#if UNITY_EDITOR
        EditorUtility.SetDirty((Node)parent);
#endif
    }

    public List<Node> GetChildren(IHaveChildren parent)
    {
        return parent.GetChildren();
    }

}
