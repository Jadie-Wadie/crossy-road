using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLaneScript : PopulateScript
{
	[Header("GameObjects")]
	public GameObject[] obstacles;

    public override void Populate(LaneType? type = null, LaneScript laneScript = null) {
		PopulateEnds();	
	}

	// Spawn Trees at Ends
	public void PopulateEnds() {
		void SpawnTree(int x, Transform parent) {
			if (Random.Range(0, 101) < 95) {
				GameObject obstacle = Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform.position + new Vector3(x + 0.5f, 0, 0), Quaternion.identity);
				obstacle.transform.SetParent(parent);
			}
		}

		Transform lEnd = transform.Find("lEnd");
		Transform rEnd = transform.Find("rEnd");

		for (int i = 0; i < laneWidth; i++) {
			SpawnTree(i - Mathf.RoundToInt(laneWidth * 1.5f), lEnd);
			SpawnTree(i + Mathf.RoundToInt(laneWidth * 0.5f), rEnd);
		}
	}
}
