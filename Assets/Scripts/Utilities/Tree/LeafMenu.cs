using UnityEngine;

public class LeafMenu : LeafNode
{
    public string message;

    protected override void OnNotSelected()
    {
        selected = false;
        Debug.Log($"Not Selected: {message}");
    }

    protected override void OnSelected()
    {
        selected = true;
        Debug.Log($"Selected: {message}");
    }
}
