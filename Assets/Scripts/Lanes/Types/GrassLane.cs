using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassLane : SpawnLane
{
	/*

	[Header("GameObjects")]
	public Vector2Int chance;

	[Space(10)]

	private List<int> spawns;
	
	public override void Populate()
	{
		// Populate Ends
		PopulateEnds();

		// Log Prev Lane
		
		switch (type)
		{
			default:
				break;
		}

		/*

		// Initialise List
		spawns = new List<int>();
		for (int i = -Mathf.RoundToInt(laneWidth / 2) + 1; i < Mathf.RoundToInt(laneWidth / 2) + 1; i++)
		{
			spawns.Add(i);
		}

		// Generate Obstacles
		int count = Random.Range(chance.x, chance.y + 1);

		for (int i = 0; i < count; i++)
		{
			int rand = Random.Range(0, spawns.Count);

			GameObject obstacle = Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform.position + new Vector3(spawns[rand] - 0.5f, 0, 0), Quaternion.identity);
			obstacle.transform.SetParent(transform);

			spawns.RemoveAt(rand);
			if (spawns.Count == 0) break;
		}

		*\/
	}

	*/
}
