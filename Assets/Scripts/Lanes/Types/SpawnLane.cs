using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLane : Lane
{
	[Header("GameObjects")]
	public Weight[] weights;

	[Space(10)]

	private List<GameObject> obstacles = new List<GameObject>();

	new public void Awake()
	{
		base.Awake();

		// Calculate Weights
		for (int i = 0; i < weights.Length; i++)
		{
			for (int j = 0; j < weights[i].weight; j++)
			{
				obstacles.Add(weights[i].value);
			}
		}
	}

	public override void Populate(Lane prevLane)
	{
		PopulateEnds();
	}

	// Spawn Obstacles at Ends
	public void PopulateEnds()
	{
		for (int i = 0; i < laneWidth; i++)
		{
			SpawnObstacle(i - Mathf.RoundToInt(laneWidth * 1.5f), lEnd.transform, 90);
			SpawnObstacle(i + Mathf.RoundToInt(laneWidth * 0.5f), rEnd.transform, 90);
		}
	}

	// Spawn Obstacle
	GameObject SpawnObstacle(int position, Transform parent, int chance = 100)
	{
		GameObject obstacle = null;

		if (Random.Range(0, 100) < chance)
		{
			obstacle = Instantiate(obstacles[Random.Range(0, obstacles.Count)], transform.position + new Vector3(position + 0.5f, 0, 0), Quaternion.identity);
			obstacle.transform.SetParent(parent);
		}

		return obstacle;
	}
}
