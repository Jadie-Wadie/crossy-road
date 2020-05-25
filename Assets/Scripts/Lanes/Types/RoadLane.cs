using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		// Initialise Variables
		speed = Random.Range(speedRange.x, speedRange.y) * (Random.Range(0, 2) == 0 ? -1 : 1);
		spawnPos = new Vector3(laneWidth * 1.5f * -Mathf.Clamp(speed, -1, 1), 0, transform.position.z);

		// Generate Vehicles
		float position = spawnPos.x + Random.Range(spawnRate.x, spawnRate.y) * speed;
		while (Mathf.Abs(position) < laneWidth * 3)
		{
			GameObject vehicle = SpawnVehicle(new Vector3(position, 0, transform.position.z));
			vehicleObjects.Add(vehicle);

			position += Random.Range(spawnRate.x, spawnRate.y) * speed;
		}

		// Spawning Cycle
		StartCoroutine(SpawnCycle());
	}

	public GameObject SpawnVehicle(Vector3 position)
	{
		// Instantiate
		GameObject vehicle = Instantiate(vehicleList.random, position, Quaternion.identity);
		vehicle.transform.parent = main.transform;

		// Configure
		Vehicle script = vehicle.GetComponent<Vehicle>();
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
			if (laneWidth * 1.5f < Mathf.Clamp(speed, -1, 1) * vehicle.transform.position.x)
			{
				vehicleObjects.Remove(vehicle);
				Destroy(vehicle);
			}
		}
	}
}
