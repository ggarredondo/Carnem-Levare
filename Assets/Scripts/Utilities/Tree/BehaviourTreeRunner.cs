using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;
    public List<GameObject> menus;

    private void Start()
    {
        tree.Initialize();
        tree.OnChange += ApplyChanges;
    }

    public void ChangeMenu()
    {
        tree.GoToChild();
    }

    public void Return()
    {
        tree.GoToParent();
    }

    private void ApplyChanges()
    {
        tree.GetSelected().ForEach(id => Debug.Log(id));
    }

    public void MoveRight()
    {

    }

    public void MoveLeft()
    {

    }
}
