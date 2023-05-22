using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Node))]
[CanEditMultipleObjects]
public class NodeEditor : Editor
{
    SerializedProperty ID;
    SerializedProperty isStatic;
    SerializedProperty guid;
    SerializedProperty position;

    private GUIStyle headerStyle;
    private GUIStyle labelStyle;
    private GUIStyle buttonStyle;

    private void Initilize()
    {
        ID = serializedObject.FindProperty("ID");
        isStatic = serializedObject.FindProperty("isStatic");
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

        buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 12,
            fixedHeight = 30,
            margin = new RectOffset(0, 0, 10, 0)
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

        Header("General");
        Label("ID: ", ID.intValue.ToString());
        Label("GUID: ", guid.stringValue);
        Label("Position: ", position.vector2Value.ToString());
        DrawHorizontalGUILine();
        Header("Specific");
        Property(ref isStatic);

        serializedObject.ApplyModifiedProperties();
    }
}
