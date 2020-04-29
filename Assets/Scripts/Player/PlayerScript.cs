using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerScript : MonoBehaviour
{
	[Header("Control")]
	public GameObject model;

	[Space(10)]

	public int direction = 1;

	[Space(10)]

	public bool isJumping;

	[Header("GameObjects")]
	public GameObject[] models;

	[Header("Animation")]
	private Animator animator;

	[Header("Input")]
	public bool buttonPressed;
	public bool buttonDown;
	public bool buttonReleased;

	void Start()
	{
		model = transform.Find("Model").gameObject;
		animator = GetComponent<Animator>();

		SpawnCharacter();
	}

	void Update()
	{
		// Handle Input
		HandleInput();

		// Jumping
		if (isJumping) {
			// Move
			transform.Translate(model.transform.forward * 6 * Time.deltaTime);
		} else {
			// Look
			model.transform.rotation = Quaternion.Euler(0, 90 * direction, 0);
		}

		// Update Animation
		animator.SetBool("buttonDown", buttonDown);
		animator.SetBool("buttonReleased", buttonReleased);
	}

	void SpawnCharacter()
	{
		// Spawn a Character Prefab
		int rand = Random.Range(0, models.Length);

		GameObject character = Instantiate(models[rand], transform.position, Quaternion.identity);
		character.transform.SetParent(model.transform);

		character.transform.localRotation = Quaternion.Euler(0, 90, 0);
	}

	void HandleInput() {
		// Input Bools
		buttonPressed = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D);
		buttonDown = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
		buttonReleased = Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D);
		
		// Update Look Direction
		if (Input.GetKeyDown(KeyCode.W)) {
			direction = 0;
		} else if (Input.GetKeyDown(KeyCode.A)) {
			direction = 3;
		} else if (Input.GetKeyDown(KeyCode.S)) {
			direction = 2;
		} else if (Input.GetKeyDown(KeyCode.D)) {
			direction = 1;
		}
	}

	public void JumpComplete() {
		transform.position = new Vector3((float)Math.Round(transform.position.x), 1, (float)Math.Round(transform.position.z));
	}
}
