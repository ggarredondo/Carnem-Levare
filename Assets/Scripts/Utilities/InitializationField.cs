using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Field will be Read-Only in play mode.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public sealed class InitializationField : PropertyAttribute {}

#if UNITY_EDITOR
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
#endif