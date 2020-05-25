using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrassLane : Lane
{
	[Header("Control")]
	public int obstacleCount = 2;

	[Range(0, 100)]
	public int obstacleDensity = 90;

	[Header("Obstacles")]
	public Weight[] obstacles;
	private WeightedList obstacleList;

	new public void Awake()
	{
		obstacleList = new WeightedList(obstacles);
		base.Awake();
	}

	public override void Populate(Lane prevLane)
	{
		// Spawn Obstacles on Edges
		for (int i = 0; i < laneWidth; i++)
		{
			if (Random.Range(0, 100) < obstacleDensity) SpawnObstacle(i - Mathf.RoundToInt(laneWidth), lEnd.transform, false);
			if (Random.Range(0, 100) < obstacleDensity) SpawnObstacle(i + Mathf.RoundToInt(laneWidth), rEnd.transform, false);
		}

		// Create Valid Position List
		List<int> positions = Enumerable.Range(0, laneWidth / 2 - 1)
			.Concat(Enumerable.Range(laneWidth / 2 + 1, laneWidth / 2 - 1))
			.ToList();

		if (prevLane != null && prevLane.GetType() == typeof(GrassLane))
		{
			if (transform.position.z != 0 && prevLane.objects[laneWidth / 2 - 1] == null && prevLane.objects[laneWidth / 2] == null)
			{
				positions.Add(Random.Range(laneWidth / 2 - 1, laneWidth / 2 + 1));
			}
		}

		// Spawn Obstacles in Center
		for (int i = 0; i < obstacleCount; i++)
		{
			int position = positions[Random.Range(0, positions.Count)];
			positions.Remove(position);

			SpawnObstacle(position, main.transform, true);

			if (positions.Count == 0) break;
		}
	}

	// Spawn Obstacle
	GameObject SpawnObstacle(int position, Transform parent, bool isObject)
	{
		GameObject obstacle = Instantiate(obstacleList.random, transform.position + new Vector3(0.5f - laneWidth / 2 + position, 0, 0), Quaternion.identity);
		obstacle.transform.SetParent(parent);

		if (isObject) objects[position] = obstacle;
		return obstacle;
	}
}
