using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public System.Action<NodeView> OnNodeSelected;
    public Node node;
    public Port input;
    public Port output;

    public NodeView(Node node) : base("Assets/Editor/NodeView.uxml")
    {
        this.node = node;
        title = node.currentName;
        viewDataKey = node.guid;

        style.left = node.position.x;
        style.top = node.position.y;
        style.backgroundColor = node.backgroundColor;
        style.color = node.textColor;

        CreateInputPorts();
        CreateOutputPorts();
        SetUpClasses();
    }

    private void SetUpClasses()
    {
        if(node is CompositeNode)
        {
            AddToClassList("composite");
        }
        else if(node is LeafNode)
        {
            AddToClassList("leaf");
        }
        else if(node is DecoratorNode)
        {
            AddToClassList("decorator");
        }
        else if(node is RootNode)
        {
            AddToClassList("root");
        }
    }

    private void CreateInputPorts()
    {
        if (node is IHaveParent)
        {
            input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }

        if (input != null)
        {
            input.portName = "";
            input.style.flexDirection = FlexDirection.Column;
            inputContainer.Add(input);
        }
    }

    private void CreateOutputPorts()
    {
        if (node is CompositeNode)
        {
            output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
        }
        else if(node is DecoratorNode)
        {
            output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        else if (node is RootNode)
        {
            output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }

        if (output != null)
        {
            output.portName = "";
            output.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(output);
        }
    }

    public void UpdateStyle()
    {
        title = node.currentName;
        style.backgroundColor = node.backgroundColor;
        style.color = node.textColor;
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        Undo.RecordObject(node, "Behaviour Tree (Set position)");
        node.position.x = newPos.xMin;
        node.position.y = newPos.yMin;
        EditorUtility.SetDirty(node);
    }

    public override void OnSelected()
    {
        base.OnSelected();
        OnNodeSelected?.Invoke(this);
    }

    public void UpdateState()
    {
        RemoveFromClassList("selected");
        RemoveFromClassList("not_selected");

        if (Application.isPlaying)
            if (node.selected) AddToClassList("selected");
            else AddToClassList("not_selected");
    }


}
