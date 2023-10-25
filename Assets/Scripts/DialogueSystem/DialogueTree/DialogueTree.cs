using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Tree/DialogueTree")]
public class DialogueTree : BehaviourTree
{
    private readonly Stack<Node> nodeStack = new();
    public System.Action OnChange;

    public void Initialize()
    {
        nodeStack.Clear();
        nodeStack.Push(rootNode);
        OnChange.Invoke();
    }
}
