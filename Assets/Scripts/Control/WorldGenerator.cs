using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameController))]
public class WorldGenerator : MonoBehaviour
{
	[Header("Control")]
	public int spawnLength = 1;

	[Space(10)]

	public Vector2Int buffer = new Vector2Int(5, 15);

	[Header("GameObjects")]
	public Transform laneParent;

	[Space(10)]

	public GameObject spawn;

	public Weight[] lanes;
	private WeightedList laneList;

	[Header("Debug")]
	private GameController gameController;

	[Space(10)]

	public int counter = 0;

	[Space(10)]

	private List<GameObject> laneObjects = new List<GameObject>();
	private Lane prevLane;

	void Awake()
	{
		laneList = new WeightedList(lanes);
		gameController = GetComponent<GameController>();
	}

	public void Generate()
	{
		// Destroy Lanes
		foreach (GameObject lane in laneObjects.ToArray())
		{
			laneObjects.Remove(lane);
			GameObject.Destroy(lane);
		}

		// Generate New Lanes
		counter = -buffer.x;

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

	// Spawn Lane
	void SpawnLane()
	{
		// Determine Lane Type
		GameObject lanePrefab = laneList.random;
		if (counter < spawnLength) lanePrefab = spawn;

		// Spawn the Lane
		GameObject laneObject = Instantiate(lanePrefab, new Vector3(0, 0, counter), Quaternion.identity);
		laneObject.transform.SetParent(laneParent);

		// Set Lane Variant
		Lane lane = laneObject.GetComponent<Lane>();
		if (lane.variants.Length != 0) lane.SetVariant(Mathf.Abs(counter) % lane.variants.Length);

		// Populate the Lane
		lane.Populate(prevLane);
		prevLane = lane;

		// Increase Counter
		laneObjects.Add(laneObject);
		counter++;
	}
}