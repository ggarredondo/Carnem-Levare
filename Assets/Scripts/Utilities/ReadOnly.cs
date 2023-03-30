using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Field will be Read-Only in play mode.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public sealed class ReadOnly : PropertyAttribute {}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnly))]
public sealed class ReadOnlyFieldDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
#endif