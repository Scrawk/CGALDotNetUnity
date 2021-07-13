using System;
using UnityEditor;
using UnityEngine;

using Common.Core.Numerics;

[CustomPropertyDrawer(typeof(Vector2))]
[CustomPropertyDrawer(typeof(Vector2Int))]
[CustomPropertyDrawer(typeof(Vector2f))]
[CustomPropertyDrawer(typeof(Vector2d))]
[CustomPropertyDrawer(typeof(Vector2i))]
public class Vector2Drawer : PropertyDrawer
{

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        label = EditorGUI.BeginProperty(pos, label, prop);
        var contentRect = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);

        var labels = new[]
        {
            new GUIContent("X"),
            new GUIContent("Y")
        };

        var properties = new[]
        {
            prop.FindPropertyRelative("x"),
            prop.FindPropertyRelative("y")
        };

        PropertyDrawerUtil.DrawPropertyFieldsHorizontal(contentRect, labels, properties);

        EditorGUI.EndProperty();
    }

}
