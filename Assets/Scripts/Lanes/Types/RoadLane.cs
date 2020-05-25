using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
	Left, Right
}

public class RoadLane : Lane
{
	[Header("Control")]
	public Vector2 speedRange = new Vector2(2.75f, 3.25f);

	[Header("Decoration")]
	public GameObject linePrefab;

	[Header("Vehicles")]
	public Vector2 spawnRate = new Vector2(2, 3);

	public Weight[] vehicles;
	private WeightedList vehicleList;

	private List<GameObject> vehicleObjects = new List<GameObject>();

	[Header("Debug")]
	public Direction direction;
	public float speed;

	[Space(10)]

	private Vector3 spawnPos;
	public bool isSpawning = true;

	new public void Awake()
	{
		vehicleList = new WeightedList(vehicles);
		base.Awake();
	}

	override public void Populate(Lane prevLane)
	{
		// Generate Lines
		if (prevLane != null && prevLane.GetType() == typeof(RoadLane))
		{
			for (int i = 0; i < Mathf.Round(laneWidth * 1.5f); i++)
			{
				GameObject line = Instantiate(linePrefab, transform.position + new Vector3((i - 1) * 2 - laneWidth - 0.5f, 0.46f, -0.5f), Quaternion.identity);
				line.transform.SetParent(main.transform);
			}
		}

		// Direction and Speed
		// float delay = Random.Range(0, spawnRate.y);

		direction = Random.Range(0, 2) == 0 ? Direction.Left : Direction.Right;
		speed = Random.Range(speedRange.x, speedRange.y);

		spawnPos = new Vector3(laneWidth * 1.5f * (direction == Direction.Left ? 1 : -1), 0, transform.position.z);

		// Generate Vehicles
		float position = spawnPos.x + Random.Range(spawnRate.x, spawnRate.y) * speed * (direction == Direction.Left ? -1 : 1);
		while (Mathf.Abs(position) < laneWidth * 3)
		{
			GameObject vehicle = SpawnVehicle(new Vector3(position, 0, transform.position.z));
			vehicleObjects.Add(vehicle);

			position += Random.Range(spawnRate.x, spawnRate.y) * speed * (direction == Direction.Left ? -1 : 1);
		}

		// Spawning Cycle
		StartCoroutine(SpawnCycle());
	}

	public GameObject SpawnVehicle(Vector3 position)
	{
		// Spawn a Vehicle
		GameObject vehicle = Instantiate(vehicleList.random, position, Quaternion.identity);
		vehicle.transform.parent = main.transform;

		// Set Variables on Script
		Vehicle script = vehicle.GetComponent<Vehicle>();
		script.direction = direction;
		script.speed = speed;

		return vehicle;
	}

	public IEnumerator SpawnCycle()
	{
		// Spawn a Vehicle
		GameObject vehicle = SpawnVehicle(spawnPos);
		vehicleObjects.Add(vehicle);

		// Wait for SpawnRate
		yield return new WaitForSeconds(Random.Range(spawnRate.x, spawnRate.y + 1));
		StartCoroutine(SpawnCycle());
	}

	void Update()
	{
		// Destroy Vehicles
		foreach (GameObject vehicle in vehicleObjects.ToArray())
		{
			if (direction == Direction.Left ? (vehicle.transform.position.x < -laneWidth * 1.5f) : (laneWidth * 1.5f < vehicle.transform.position.x))
			{
				vehicleObjects.Remove(vehicle);
				Destroy(vehicle);
			}
		}
	}
}
