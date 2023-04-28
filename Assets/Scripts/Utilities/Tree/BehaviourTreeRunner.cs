using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    public InputReader input;
    public BehaviourTree tree;
    public List<GameObject> menus;

    private void Start()
    {
        tree.Initialize();
        tree.OnChange += ApplyChanges;
        input.ChangeRightMenuEvent += MoveRight;
        input.ChangeLeftMenuEvent += MoveLeft;
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
        tree.GetSelected().ForEach(id => Debug.Log("Selected: " + id));
    }

    public void MoveRight()
    {
        tree.MoveToRightSibling();
    }

    public void MoveLeft()
    {
        tree.MoveToLeftSibling();
    }
}
