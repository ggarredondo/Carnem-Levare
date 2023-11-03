using MenuTreeUtilities;
using UnityEngine;

public class DebugMenuController : MonoBehaviour
{
    public MenuTree tree;

    private void Start()
    {
        tree.OnChange += Empty;
        tree.Initialize();
    }

    private void Empty()
    {
        tree.GetSelected();
    }

    public void ChangeMenu()
    {
        tree.GoToChild();
    }

    public void ChangeSibling(int child)
    {
        if (tree.ChangeSibling(child))
        {

        }
    }

    public void MoveRight()
    {
        if (tree.MoveToRightSibling())
        {

        }
    }

    public void MoveLeft()
    {
        if (tree.MoveToLeftSibling())
        {

        }
    }

    public void GoToParent()
    {
        tree.GoToParent();
    }
}
