using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLane : Lane
{
	[Header("Control")]
	public Vector2 speedRange = new Vector2(2.75f, 3.25f);

	[Header("Logs")]
	public Vector2 spawnRate = new Vector2(2, 3);

	public Weight[] logs;
	private WeightedList logList;

	private List<GameObject> logObjects = new List<GameObject>();

	[Header("Debug")]
	public float speed;

	[Space(10)]

	private Vector3 spawnPos;
	public bool isSpawning = true;

	new public void Awake()
	{
		logList = new WeightedList(logs);
		base.Awake();

		main.transform.localScale += new Vector3(0f, -0.2f, 0f);
		lEnd.transform.localScale += new Vector3(0f, -0.2f, 0f);
		rEnd.transform.localScale += new Vector3(0f, -0.2f, 0f);

		main.transform.position += new Vector3(0f, -0.1f, 0f);
		lEnd.transform.position += new Vector3(0f, -0.1f, 0f);
		rEnd.transform.position += new Vector3(0f, -0.1f, 0f);
	}

	override public void Populate(Lane prevLane)
	{
		// Initialise Variables
		speed = Random.Range(speedRange.x, speedRange.y) * (Random.Range(0, 2) == 0 ? -1 : 1);
		if (prevLane != null && prevLane.GetType() == typeof(WaterLane))
		{
			// Ensure Alternating Water Lanes
			if (Mathf.Clamp(((WaterLane)prevLane).speed, -1, 1) == Mathf.Clamp(speed, -1, 1)) speed *= -1;
		}

		spawnPos = new Vector3(laneWidth * 1.5f * -Mathf.Clamp(speed, -1, 1), 0, transform.position.z);

		// Generate Logs
		logObjects.Add(SpawnLog(new Vector3(laneWidth / 2 + Random.Range(0f, 1f) * speed, 0, transform.position.z)));
		logObjects.Add(SpawnLog(new Vector3(-laneWidth / 2 + Random.Range(0f, 1f) * speed, 0, transform.position.z)));

		// Tag Main
		main.tag = "Water";

		// Spawning Cycle
		StartCoroutine(SpawnCycle());
	}

	public GameObject SpawnLog(Vector3 position)
	{
		// Instantiate
		GameObject log = Instantiate(logList.random, position, Quaternion.identity);
		log.transform.parent = main.transform;

		// Configure
		Translate script = log.GetComponent<Translate>();
		script.speed = speed;
		script.laneWidth = laneWidth;

		return log;
	}

	public IEnumerator SpawnCycle()
	{
		// Spawn a Log
		GameObject log = SpawnLog(spawnPos);
		logObjects.Add(log);

		log.GetComponent<Translate>().boost = true;

		// Wait for SpawnRate
		yield return new WaitForSeconds(Random.Range(spawnRate.x, spawnRate.y + 1));
		StartCoroutine(SpawnCycle());
	}

	void Update()
	{
		// Destroy Logs
		foreach (GameObject log in logObjects.ToArray())
		{
			if (laneWidth * 1.5f < Mathf.Clamp(speed, -1, 1) * log.transform.position.x)
			{
				logObjects.Remove(log);
				Destroy(log);
			}
		}
	}
}
