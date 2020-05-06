using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassLaneScript : PopulateScript
{
	[Header("GameObjects")]
	public int max = 3;
	public GameObject[] obstacles;

	[Space(10)]

	private List<int> spawns;

    public override void Populate(LaneType? type = null, LaneScript laneScript = null) {
		// Initialise List
		spawns = new List<int>();
		for (int i = -3; i < 5; i++) {
			spawns.Add(i);
		}

		// Generate Obstacles
		int count = Random.Range(0, max);

		for (int i = 0; i < count; i++) {
			int rand = Random.Range(0, spawns.Count);

			GameObject obstacle = Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform.position + new Vector3(spawns[rand] - 0.5f, 0, 0), Quaternion.identity);
			obstacle.transform.SetParent(transform);

			spawns.RemoveAt(rand);
			if (spawns.Count == 0) break;
		}
	}
}
