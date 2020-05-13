using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LaneType
{
	public GameObject prefab;

	[Space(10)]

	public int weight;
	public bool spawn;
}

public class WorldGenerator : MonoBehaviour
{
	[Header("Control")]
	public LaneType[] lanes;

	[Space(10)]

	public Transform laneParent;

	[Header("Generation")]
	public int counter = 0;

	[Space(10)]

	public int spawnLength = 1;

	[Space(10)]

	public Vector2Int buffer = new Vector2Int(5, 15);
	public Vector3 offset;

	[Header("Debug")]
	private List<LaneType> weights = new List<LaneType>();

	[Space(10)]

	private SpawnedLane prevLane;

	void Start()
	{
		// Initalise Variables
		counter = -buffer.x;
		CalculateWeights();

		// Generate Inital Lanes
		for (int i = -buffer.x; i < buffer.y; i++)
		{
			SpawnLane();
		}
	}

	// Calculate Lane Type Weightings
	void CalculateWeights()
	{
		for (int i = 0; i < lanes.Length; i++)
		{
			for (int j = 0; j < lanes[i].weight; j++)
			{
				weights.Add(lanes[i]);
			}
		}
	}

	// Spawn Lane
	void SpawnLane()
	{
		// Determine Lane Type (Respecting Spawn Length)
		LaneType laneType = weights[Random.Range(0, weights.Count)];

		if (counter < spawnLength)
		{
			for (int i = 0; i < lanes.Length; i++)
			{
				if (lanes[i].spawn)
				{
					laneType = lanes[i];
					break;
				}
			}
		}

		// Spawn the Lane
		GameObject laneObject = Instantiate(laneType.prefab, new Vector3(0, 0, counter) + offset, Quaternion.identity);
		laneObject.transform.SetParent(laneParent);

		// Set Lane Variant
		Lane lane = laneObject.GetComponent<Lane>();
		if (lane.variants.Length != 0)
		{
			int variant = Mathf.Abs(counter) % (lane.variants.Length + 1);
			if (variant != 0) lane.SetVariant(variant);
		}

		// Populate the Lane
		Debug.Log($"Spawned a {lane.GetType()}");

		// Increase Counter
		counter++;
	}
}