using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassLane : SpawnLane
{
	public override void Populate(Lane prevLane)
	{
		// Populate Ends
		PopulateEnds();

		// Log Prev Lane
		switch (prevLane)
		{
			case GrassLane grass:
				Debug.Log("Grass");
				break;

			case SpawnLane spawn:
				Debug.Log("Spawn");
				break;

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
		*/
	}
}
