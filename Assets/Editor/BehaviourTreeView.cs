using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeView : GraphView
{
    public System.Action<NodeView> OnNodeSelected;
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, UxmlTraits> { }
    public BehaviourTree tree;
    private Vector2 localMousePosition;

    public BehaviourTreeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);

        Undo.undoRedoPerformed += OnUndoRedo;
        RegisterCallback<MouseMoveEvent>(OnMouseMove);
    }

    private void OnUndoRedo()
    {
        PopulateView(tree);
        AssetDatabase.SaveAssets();
    }

    NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    internal void PopulateView(BehaviourTree tree)
    {
        this.tree = tree;

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        if(tree.rootNode == null)
        {
            tree.rootNode = tree.CreateNode(typeof(RootNode), new Vector2(0,0)) as RootNode;
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }

        tree.nodes.ForEach(n => CreateNodeView(ref n));

        UpdateEdges();

        tree.AsignID();
    }

    private void UpdateEdges()
    {
        tree.nodes.ForEach(n =>
        {
            if (n is IHaveChildren children1)
            {
                var children = tree.GetChildren(children1);

                if (children[0] != null)
                {
                    float cont = 0;
                    children.ForEach(c =>
                    {
                        NodeView parentView = FindNodeView(n);
                        NodeView childView = FindNodeView(c);

                        EdgeView edge = parentView.output.ConnectTo<EdgeView>(childView.input);
                        Color newColor = GetNewColor(cont);
                        edge.UpdateColor(newColor, Color.white);
                        AddElement(edge);

                        cont += 1.0f / children.Count;
                    });
                }
            }
        });
    }

    public void OnUpdate()
    {
        PopulateView(tree);
    }

    private Color GetNewRainbowColor(float percentage)
    {
        Gradient gradient = new();
        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;

        colorKey = new GradientColorKey[5]; 
        colorKey[0].color = Color.white;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.red;
        colorKey[1].time = 0.25f;
        colorKey[2].color = Color.green;
        colorKey[2].time = 0.50f;
        colorKey[3].color = Color.blue;
        colorKey[3].time = 0.75f;
        colorKey[4].color = Color.magenta;
        colorKey[4].time = 1f;

        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);

        return gradient.Evaluate(percentage);
    }

    private Color GetNewColor(float percentage)
    {
        Gradient gradient = new();
        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;

        colorKey = new GradientColorKey[2];
        colorKey[0].color = new Color32(255, 250, 227, 255);
        colorKey[0].time = 0.0f;
        colorKey[1].color = new Color32(163, 22, 33, 255);
        colorKey[1].time = 1.0f;

        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);

        return gradient.Evaluate(percentage);
    }

    private void OnMouseMove(MouseMoveEvent e)
    {
        localMousePosition = (e.currentTarget as GraphView).contentViewContainer.WorldToLocal(e.mousePosition);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort =>
        endPort.direction != startPort.direction &&
        endPort.node != startPort.node).ToList();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        graphViewChange.elementsToRemove?.ForEach(elem =>
        {
            if (elem is NodeView nodeView)
            {
                tree.DeleteNode(nodeView.node);
            }

            if (elem is Edge edge)
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                tree.RemoveChild((IHaveChildren)parentView.node, (IHaveParent)childView.node);
            }
        });

        graphViewChange.edgesToCreate?.ForEach(edge =>
        {
            NodeView parentView = edge.output.node as NodeView;
            NodeView childView = edge.input.node as NodeView;
            tree.AddChild((IHaveChildren) parentView.node, (IHaveParent) childView.node);
        });

        return graphViewChange;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        foreach (var type in TypeCache.GetTypesDerivedFrom<LeafNode>())
            evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type, localMousePosition));

        foreach (var type in TypeCache.GetTypesDerivedFrom<CompositeNode>())
            evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type, localMousePosition));     
    }

    void CreateNode(System.Type type, Vector2 position)
    {
        Node node = tree.CreateNode(type, position);
        CreateNodeView(ref node);
    }

    void CreateNodeView(ref Node node)
    {
        NodeView nodeView = new(node);
        nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(nodeView);
    }

    public void UpdateNodeState()
    {
        nodes.ForEach(n =>
        {
            NodeView view = n as NodeView;
            view.UpdateState();
        });
    }
}
