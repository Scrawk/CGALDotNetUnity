using System;
using UnityEditor;
using UnityEngine;

using Common.Core.Numerics;

[CustomPropertyDrawer(typeof(Vector4))]
[CustomPropertyDrawer(typeof(Vector4f))]
[CustomPropertyDrawer(typeof(Vector4d))]
[CustomPropertyDrawer(typeof(Vector4i))]
[CustomPropertyDrawer(typeof(Quaternion))]
[CustomPropertyDrawer(typeof(Quaternion3f))]
[CustomPropertyDrawer(typeof(Quaternion3d))]
public class Vector4Drawer : PropertyDrawer
{

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        label = EditorGUI.BeginProperty(pos, label, prop);
        var contentRect = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);

        var labels = new[]
        {
            new GUIContent("X"),
            new GUIContent("Y"),
            new GUIContent("Z"),
            new GUIContent("W")
        };

        var properties = new[]
        {
            prop.FindPropertyRelative("x"),
            prop.FindPropertyRelative("y"),
            prop.FindPropertyRelative("z"),
            prop.FindPropertyRelative("w")
        };

        PropertyDrawerUtil.DrawPropertyFieldsHorizontal(contentRect, labels, properties);

        EditorGUI.EndProperty();
    }

}

