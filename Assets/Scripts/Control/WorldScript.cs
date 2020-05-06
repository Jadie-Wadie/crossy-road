using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LaneType {
	public string name;

	[Space(10)]

	public int weight;
	public bool spawn;

	[Space(10)]

	public GameObject prefab;
}

public class WorldScript : MonoBehaviour
{
	[Header("Control")]
	public LaneType[] types;

	[Space(10)]

	public Transform laneParent;

	[Header("Generation")]
	public int counter = 0;
	
	[Space(10)]

	public int spawnLength = 1;
	public Vector2Int buffer = new Vector2Int(5, 15);

	[Space(10)]

	public Vector3 offset;

	[Space(10)]

	public List<int> weights = new List<int>();

	[Space(10)]

	private (LaneType type, LaneScript script) prevLane;

    void Start()
    {
		// Initalise Variables
		counter = -buffer.x;
		CalculateWeights();

		// Generate Inital Lanes
		for (int i = -buffer.x; i < buffer.y; i++) {
			CreateNewLane();
		}
    }

	// Calculate Lane Type Weightings
	void CalculateWeights() {
		for (int i = 0; i < types.Length; i++) {
			for (int j = 0; j < types[i].weight; j++) {
				weights.Add(i);
			}
		}
	}

	// Generate a New Lane
    void CreateNewLane() {
		// Calculate Type
		LaneType type = types[weights[Random.Range(0, weights.Count)]];

		if (counter < spawnLength) {
			for (int i = 0; i < types.Length; i++) {
				if (types[i].spawn) { 
					type = types[i];
					break;
				}
			}
		}

		// Instantiate
		GameObject lane = Instantiate(type.prefab, offset + new Vector3(0, 0, counter++), Quaternion.identity);
		lane.transform.SetParent(laneParent);

		LaneScript laneScript = lane.GetComponent<LaneScript>();

		if (laneScript.variants.Length != 0) {
			int variant = Mathf.Abs(counter) % (laneScript.variants.Length + 1);
			if (variant != 0) laneScript.SetVariant(variant);
		}

		laneScript.Populate(prevLane.type, prevLane.script);
		prevLane = (type, laneScript);
	}
}