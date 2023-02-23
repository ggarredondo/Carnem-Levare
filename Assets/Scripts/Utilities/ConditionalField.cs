using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Field)]
public sealed class ConditionalField : PropertyAttribute
{
    private string field;
    public string Field { get => field; }

    /// <summary>
    /// Show/hide field in inspector given a boolean field.
    /// </summary>
    /// <param name="field">Bool field</param>
    public ConditionalField(string field) => this.field = field;
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ConditionalField))]
public sealed class ConditionalDrawer : PropertyDrawer
{
    private bool condition;
    private float propertyHeight;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return propertyHeight;
    }

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalField conditionalField = attribute as ConditionalField;
        condition = property.serializedObject.FindProperty(conditionalField.Field).boolValue;

        if (condition) {
            propertyHeight = base.GetPropertyHeight(property, label);
            EditorGUI.PropertyField(position, property, label);
        }
        else
            propertyHeight = 0f;
    }
}
#endif
