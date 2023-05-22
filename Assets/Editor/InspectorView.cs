using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> { }

    Editor editor;

    public InspectorView()
    {

    }

    internal void UpdateSelection(NodeView nodeView)
    {
        Clear();

        UnityEngine.Object.DestroyImmediate(editor);

        if (Application.isPlaying) Debug.Log(nodeView.node);
        editor = Editor.CreateEditor(nodeView.node, typeof(NodeEditor));

        IMGUIContainer container = new(() => 
        {
            if (editor.target) editor.OnInspectorGUI();
        });

        Add(container);
    }
}
