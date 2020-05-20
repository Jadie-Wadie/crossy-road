using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadLane : Lane
{
	[Header("Decoration")]
	public GameObject linePrefab;

	override public void Populate(Lane prevLane)
	{
		// Generate Lines
		if (prevLane.GetType() == typeof(RoadLane))
		{
			for (int i = 0; i < Mathf.Round(laneWidth * 1.5f); i++)
			{
				GameObject line = Instantiate(linePrefab, transform.position + new Vector3((i - 1) * 2 - laneWidth - 0.5f, 0.46f, -0.5f), Quaternion.identity);
				line.transform.SetParent(main.transform);
			}
		}
	}
}
