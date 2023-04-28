using UnityEngine;

public class SelectableMenu : CompositeNode, ICanSelect
{
    int selectedChild;

    public Node GetSelectedChild()
    {
        return children[selectedChild];
    }

    public void SelectChild(int child)
    {
        selectedChild = child % children.Count;
        Debug.Log(selectedChild);
    }
}
