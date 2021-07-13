using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class PropertyDrawerUtil
{

    public const int SUB_LABEL_SPACING = 4;

    public const int LABEL_OFFSET = 54;

    public static void DrawPropertyFieldsHorizontal(Rect pos, GUIContent[] subLabels, SerializedProperty[] props)
    {
        // backup gui settings
        var indent = EditorGUI.indentLevel;
        var labelWidth = EditorGUIUtility.labelWidth;

        // draw properties
        var propsCount = props.Length;
        var width = (pos.width - (propsCount - 1) * SUB_LABEL_SPACING) / propsCount;
        var contentPos = new Rect(pos.x, pos.y, width, pos.height);
        EditorGUI.indentLevel = 0;

        for (var i = 0; i < propsCount; i++)
        {
            EditorGUIUtility.labelWidth = EditorStyles.label.CalcSize(subLabels[i]).x;
            EditorGUI.PropertyField(contentPos, props[i], subLabels[i]);
            contentPos.x += width + SUB_LABEL_SPACING;
        }

        // restore gui settings
        EditorGUIUtility.labelWidth = labelWidth;
        EditorGUI.indentLevel = indent;
    }

    public static void DrawPropertyFieldsVertical(Rect pos, GUIContent[] subLabels, SerializedProperty[] props)
    {
        // backup gui settings
        var indent = EditorGUI.indentLevel;
        var labelWidth = EditorGUIUtility.labelWidth;

        // draw properties
        var propsCount = props.Length;
        EditorGUI.indentLevel = 0;

        pos.height = EditorGUIUtility.singleLineHeight;
        float space = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        for (var i = 0; i < propsCount; i++)
        {
            float labelOffset = Mathf.Max(LABEL_OFFSET, EditorStyles.label.CalcSize(subLabels[i]).x);
            float heightOffset = space * (i+1);

            var contentPos = new Rect(pos.x - labelOffset, pos.y + heightOffset, pos.width + labelOffset, pos.height);

            EditorGUIUtility.labelWidth = labelOffset - 2;
            EditorGUI.PropertyField(contentPos, props[i], subLabels[i]);
        }

        // restore gui settings
        EditorGUIUtility.labelWidth = labelWidth;
        EditorGUI.indentLevel = indent;
    }

}
