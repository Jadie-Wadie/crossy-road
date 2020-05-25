using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct Weight
{
	public int weight;
	public GameObject prefab;
}

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

[System.Serializable]
public class WeightedList
{
	private List<GameObject> values { get; } = new List<GameObject>();
	public GameObject random { get { return values[Random.Range(0, values.Count)]; } }

	public WeightedList(Weight[] weights)
	{
		for (int i = 0; i < weights.Length; i++)
		{
			for (int j = 0; j < weights[i].weight; j++)
			{
				values.Add(weights[i].prefab);
			}
		}
	}
}
