using UnityEditor;

[CustomEditor(typeof(Node))]
[CanEditMultipleObjects]
public class NodeEditor : Editor
{
    SerializedProperty ID;
    SerializedProperty isStatic;

    private void Initilize()
    {
        ID = serializedObject.FindProperty("ID");
        isStatic = serializedObject.FindProperty("isStatic");
    }

    public override void OnInspectorGUI()
    {
        Initilize();
        serializedObject.Update();
        EditorGUILayout.PropertyField(ID);
        if(isStatic != null) EditorGUILayout.PropertyField(isStatic);
        serializedObject.ApplyModifiedProperties();
    }
}
