using System;
using UnityEditor;
using UnityEngine;

using Common.Geometry.Shapes;

[CustomPropertyDrawer(typeof(Box2i))]
[CustomPropertyDrawer(typeof(Box2f))]
[CustomPropertyDrawer(typeof(Box2d))]
[CustomPropertyDrawer(typeof(Box3i))]
[CustomPropertyDrawer(typeof(Box3f))]
[CustomPropertyDrawer(typeof(Box3d))]
public class BoxDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 3;
    }

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        label = EditorGUI.BeginProperty(pos, label, prop);
        var contentRect = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);

        var labels = new[]
        {
            new GUIContent("Min"),
            new GUIContent("Max")
        };

        var properties = new[]
        {
            prop.FindPropertyRelative("Min"),
            prop.FindPropertyRelative("Max")
        };

        PropertyDrawerUtil.DrawPropertyFieldsVertical(contentRect, labels, properties);

        EditorGUI.EndProperty();
    }
}

