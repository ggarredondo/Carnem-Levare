using UnityEngine;
using System;
using UnityEditor;

/// <summary>
/// Field will be Read-Only in play mode.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public sealed class InitializationField : PropertyAttribute {}

[CustomPropertyDrawer(typeof(InitializationField))]
public sealed class InitializationFieldDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = !Application.isPlaying;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
