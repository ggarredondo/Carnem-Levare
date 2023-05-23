using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EdgeView : Edge
{
    public EdgeView() : base()
    {
    }

    public override void OnPortChanged(bool isInput)
    {
        base.OnPortChanged(isInput);
    }

    public void UpdateColor(Color inputColor, Color outputColor)
    {
        if (input != null)
            input.portColor = inputColor;
        if (output != null)
            output.portColor = outputColor;
    }
}
