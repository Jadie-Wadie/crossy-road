using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLane : Lane
{
	/*

	[Header("GameObjects")]
	public GameObject[] obstacles;

	public override void Populate(Lane? type = null, LaneScript laneScript = null)
	{
		PopulateEnds();
	}

	// Spawn Obstacles at Ends
	public void PopulateEnds()
	{
		Transform lEnd = transform.Find("lEnd");
		Transform rEnd = transform.Find("rEnd");

		for (int i = 0; i < laneWidth; i++)
		{
			SpawnObstacle(i - Mathf.RoundToInt(laneWidth * 1.5f), lEnd, 90);
			SpawnObstacle(i + Mathf.RoundToInt(laneWidth * 0.5f), rEnd, 90);
		}
	}

	// Spawn Obstacle
	GameObject SpawnObstacle(int position, Transform parent, int chance = 100)
	{
		GameObject obstacle = null;

		if (Random.Range(0, 100) < chance)
		{
			obstacle = Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform.position + new Vector3(position + 0.5f, 0, 0), Quaternion.identity);
			obstacle.transform.SetParent(parent);
		}

		return obstacle;
	}

	*/
}
