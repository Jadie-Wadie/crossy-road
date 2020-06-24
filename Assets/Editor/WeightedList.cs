using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Weight))]
public class ColorPointDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		label = EditorGUI.BeginProperty(position, label, property);

		Rect contentPosition = EditorGUI.PrefixLabel(position, label);
		contentPosition.width *= 0.5f;
		EditorGUI.indentLevel = 0;

		EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("weight"), GUIContent.none);
		contentPosition.x += contentPosition.width;
		EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("prefab"), GUIContent.none);

		EditorGUI.EndProperty();
	}
}
