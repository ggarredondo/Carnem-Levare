using TreeUtilities;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Node))]
[CanEditMultipleObjects]
public class DialogueNodeEditor : Editor
{
    SerializedProperty text;

    private GUIStyle textAreaStyle;
    private GUIStyle headerStyle;

    private void Initilize()
    {
        text = serializedObject.FindProperty("text");

        textAreaStyle = new GUIStyle(EditorStyles.textField)
        {
            wordWrap = true,
        };

        headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 13,
            fontStyle = FontStyle.Bold,
            margin = new RectOffset(0, 0, 10, 10)
        };
    }

    private void Header(string text)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(text, headerStyle);
        EditorGUILayout.Space();
    }

    private void TextArea(ref SerializedProperty text)
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        text.stringValue = EditorGUILayout.TextArea(text.stringValue, textAreaStyle, GUILayout.Height(100));
        EditorGUILayout.EndVertical();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Initilize();
        serializedObject.Update();

        Header("Content");
        TextArea(ref text);

        serializedObject.ApplyModifiedProperties();
    }
}
