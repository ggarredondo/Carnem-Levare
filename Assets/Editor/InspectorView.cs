using UnityEngine.UIElements;
using UnityEditor;
using UnityEngine;

public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> { }
    public event System.Action UpdateEvent;
    Editor editor;
    Editor specificEditor;

    public InspectorView()
    {
        IMGUIContainer container = new(() =>
        {
            if (GUILayout.Button("UPDATE"))
            {
                UpdateEvent?.Invoke();
            }
        });

        Add(container);
    }

    internal void UpdateSelection(NodeView nodeView)
    {
        Clear();

        Object.DestroyImmediate(editor);
        editor = Editor.CreateEditor(nodeView.node, typeof(NodeEditor));
        specificEditor = Editor.CreateEditor(nodeView.node);

        IMGUIContainer container = new(() => 
        {
            if (GUILayout.Button("UPDATE"))
            {
                UpdateEvent?.Invoke();
            }

            if (editor.target)
            {
                EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.MaxWidth(300));
                editor.OnInspectorGUI();
                specificEditor.OnInspectorGUI();
                EditorGUILayout.EndVertical();
            }
        });

        Add(container);
    }
}
