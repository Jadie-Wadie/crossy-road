using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Weight
{
	public GameObject value;
	public int weight;
}

[RequireComponent(typeof(GameController))]
public class WorldGenerator : MonoBehaviour
{
	[Header("Control")]
	public int spawnLength = 1;

	[Space(10)]

	public Vector2Int buffer = new Vector2Int(5, 15);
	public Vector3 offset;

	[Header("GameObjects")]
	public Transform laneParent;

	[Space(10)]

	public GameObject spawn;
	public Weight[] weights;

	[Space(10)]

	public List<GameObject> laneObjects = new List<GameObject>();

	[Header("Debug")]
	public GameController gameController;

	[Space(10)]

	public int counter = 0;

	[Space(10)]

	private List<GameObject> lanes = new List<GameObject>();
	private Lane prevLane;

	void Start()
	{
		// Initalise Variables
		gameController = GetComponent<GameController>();

		counter = -buffer.x;
		CalculateWeights();

		// Generate Inital Lanes
		for (int i = -buffer.x; i < buffer.y; i++)
		{
			SpawnLane();
		}
	}

	void Update()
	{
		// Calculate Player Positions
		(float back, float front) positions = (Mathf.Infinity, 0);
		foreach (GameObject player in gameController.players)
		{
			positions = (Mathf.Min(positions.back, player.transform.position.z), Mathf.Max(positions.front, player.transform.position.z));
		}

		// Create New Lanes
		if (counter < positions.front + buffer.y) SpawnLane();

		// Destroy Previous Lanes
		foreach (GameObject lane in laneObjects.ToArray())
		{
			if (lane.transform.position.z < positions.back - buffer.x)
			{
				laneObjects.Remove(lane);
				Destroy(lane);
			}
		}
	}

	// Calculate Lane Type Weightings
	void CalculateWeights()
	{
		for (int i = 0; i < weights.Length; i++)
		{
			for (int j = 0; j < weights[i].weight; j++)
			{
				lanes.Add(weights[i].value);
			}
		}
	}

	// Spawn Lane
	void SpawnLane()
	{
		// Determine Lane Type
		GameObject lanePrefab = lanes[Random.Range(0, lanes.Count)];
		if (counter < spawnLength) lanePrefab = spawn;

		// Spawn the Lane
		GameObject laneObject = Instantiate(lanePrefab, new Vector3(0, 0, counter) + offset, Quaternion.identity);
		laneObject.transform.SetParent(laneParent);

		// Set Lane Variant
		Lane lane = laneObject.GetComponent<Lane>();
		if (lane.variants.Length != 0) lane.SetVariant(Mathf.Abs(counter) % lane.variants.Length);

		// Populate the Lane
		lane.Populate(prevLane);

		// Increase Counter
		laneObjects.Add(laneObject);
		counter++;
	}
}