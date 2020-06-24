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
