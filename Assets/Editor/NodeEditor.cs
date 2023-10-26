using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Node))]
[CanEditMultipleObjects]
public class NodeEditor : Editor
{
    SerializedProperty currentName;
    SerializedProperty backgroundColor;
    SerializedProperty textColor;
    SerializedProperty ID;
    SerializedProperty guid;
    SerializedProperty position;

    private GUIStyle headerStyle;
    private GUIStyle labelStyle;

    private void Initilize()
    {
        currentName = serializedObject.FindProperty("currentName");
        backgroundColor = serializedObject.FindProperty("backgroundColor");
        textColor = serializedObject.FindProperty("textColor");
        ID = serializedObject.FindProperty("ID");
        guid = serializedObject.FindProperty("guid");
        position = serializedObject.FindProperty("position");

        headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 16,
            fontStyle = FontStyle.Bold,
            margin = new RectOffset(0, 0, 10, 10)
        };

        labelStyle = new GUIStyle(EditorStyles.label)
        {
            fontStyle = FontStyle.Italic,
            margin = new RectOffset(0, 0, 0, 5)
        };
    }

    private void DrawHorizontalGUILine(int height = 1)
    {
        GUILayout.Space(4);

        Rect rect = GUILayoutUtility.GetRect(10, height, GUILayout.ExpandWidth(true));
        rect.height = height;
        rect.xMin = 0;
        rect.xMax = EditorGUIUtility.currentViewWidth;

        Color lineColor = new Color(0.10196f, 0.10196f, 0.10196f, 1);
        EditorGUI.DrawRect(rect, lineColor);
        GUILayout.Space(4);
    }

    private void Header(string text)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(text, headerStyle);
        EditorGUILayout.Space();
    }

    private void Label(string text, string text2)
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label(text + text2, labelStyle);
        EditorGUILayout.EndVertical();
    }

    private void Property(ref SerializedProperty property)
    {
        if (property != null)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(property);
            EditorGUILayout.EndVertical();
        }
    }

    public override void OnInspectorGUI()
    {
        Initilize();
        serializedObject.Update();

        Header("Info");
        Label("ID: ", ID.intValue.ToString());
        Label("GUID: ", guid.stringValue);
        Label("Position: ", position.vector2Value.ToString());
        Header("Style");
        Property(ref currentName);
        Property(ref backgroundColor);
        Property(ref textColor);
        DrawHorizontalGUILine();
        Header("Settings");

        serializedObject.ApplyModifiedProperties();
    }
}
