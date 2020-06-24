using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class NewPlayer : MonoBehaviour
{
	[Header("Control")]
	public bool playing = true;
	public int id = 0;

	[Space(10)]

	public State state = State.Idle;
	public enum State
	{
		Idle,
		Crouch
	}

	[HideInInspector]
	public Keybinds keybinds;

	[Header("Movement")]
	public Direction direction = Direction.W;
	public enum Direction
	{
		W = 0,
		A = 3,
		S = 2,
		D = 1
	}

	[Space(10)]

	public float startTime = 0f;
	public Vector3 startSize = idleSize;

	[Header("Graphics")]
	public GameObject[] models;

	[Header("Animation")]
	static float idleTime = 0.075f;
	static Vector3 idleSize = new Vector3(1, 1, 1);

	static float crouchTime = 0.15f;
	static Vector3 crouchSize = new Vector3(1, 0.9f, 1);

	[Header("Debug")]
	private Transform model;

	void Awake()
	{
		SpawnModel();
	}

	void Start()
	{

	}

	void Update()
	{
		// Check for Playing
		if (playing)
		{
			// Switch on State
			switch (state)
			{
				case State.Idle:
					// Look for Crouch
					if (Input.GetKey(keybinds.W) ||
						Input.GetKey(keybinds.A) ||
						Input.GetKey(keybinds.S) ||
						Input.GetKey(keybinds.D)
					) StartCrouch();

					// Animate
					Animate(idleTime, idleSize);

					// Rotate
					model.rotation = Quaternion.Euler(0, (int)direction * 90, 0);
					break;

				case State.Crouch:
					// Look for Crouch
					if (Input.GetKeyUp(keybinds.W) ||
						Input.GetKeyUp(keybinds.A) ||
						Input.GetKeyUp(keybinds.S) ||
						Input.GetKeyUp(keybinds.D)
					) StartIdle();

					// Animate
					Animate(crouchTime, crouchSize);

					// Rotate
					model.rotation = Quaternion.Euler(0, (int)direction * 90, 0);
					break;
			}

			// Set Look Direction
			if (Input.GetKey(keybinds.W)) direction = Direction.W;
			if (Input.GetKey(keybinds.A)) direction = Direction.A;
			if (Input.GetKey(keybinds.S)) direction = Direction.S;
			if (Input.GetKey(keybinds.D)) direction = Direction.D;
		}
	}

	// Choose a Random Model
	void SpawnModel()
	{
		// Get the Parent
		model = transform.Find("Model");

		// Spawn the Model
		if (models.Length == 0) throw new System.IndexOutOfRangeException("No model prefabs were provided.");

		GameObject modelPrefab = Instantiate(models[Random.Range(0, models.Length)], transform.position, Quaternion.identity);
		modelPrefab.transform.SetParent(model);

		// Face Forwards
		modelPrefab.transform.localRotation = Quaternion.Euler(0, 90, 0);
	}

	// Start Animation
	void StartAnimation()
	{
		startTime = Time.time;
		startSize = model.localScale;
	}

	// Animate to a Scale
	void Animate(float time, Vector3 size)
	{
		// Elapsed Time
		float elapsed = Time.time - startTime;
		if (time < elapsed) return;

		// Scale and Position
		model.localScale = startSize + (size - startSize) * (elapsed / time);
		model.localPosition = new Vector3(0, (size.y - 1f) / 2 * (elapsed / time), 0);
	}

	// Start Idle
	void StartIdle()
	{
		state = State.Idle;
		StartAnimation();
	}

	// Start Crouch
	void StartCrouch()
	{
		state = State.Crouch;
		StartAnimation();
	}
}
