using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum PlayerState {
	Idle,
	Crouch,
	Jump
}

public enum KeyState {
	None,
	Down,
	Held,
	Up
}

public class PlayerScript : MonoBehaviour
{
	[Header("Control")]
	public GameObject model;

	[Space(10)]

	public PlayerState state = PlayerState.Idle;

	[Space(10)]

	public bool isJumping = false;
	public bool repeatJump = false;

	[Space(10)]

	public int direction = 0;
	
	[Header("GameObjects")]
	public GameObject[] models;

	[Header("Animation")]
	public float jumpSpeed;
	private Animator animator;
	
	void Start()
	{
		model = transform.Find("Model").gameObject;
		animator = GetComponent<Animator>();

		SpawnCharacter();
	}

	void Update()
	{
		Debug.Log(state);

		// Handle State
		switch (state) {
			case PlayerState.Idle:
				// Update Animator
				animator.SetBool("isCrouching", false);
				animator.SetBool("repeatJump", false);

				// Check for Crouch
				if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
					state = PlayerState.Crouch;
				}

				// Look
				model.transform.rotation = Quaternion.Euler(0, direction * 90, 0);
				break;
			case PlayerState.Crouch:
				// Update Animator
				animator.SetBool("isCrouching", true);
				animator.SetBool("repeatJump", false);

				// Check for Jump
				if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D)) {
					if (!(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))) {
						state = PlayerState.Jump;
					}
				}

				// Look
				model.transform.rotation = Quaternion.Euler(0, direction * 90, 0);
				break;
			case PlayerState.Jump:
				// Update Animator
				animator.SetBool("isCrouching", false);

				// Jumping
				if (isJumping) {
					// Move
					transform.Translate(model.transform.forward * (1 / jumpSpeed) * Time.deltaTime);

					// Check for Chaining
					if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D)) {
						repeatJump = true;
						animator.SetBool("repeatJump", true);
					}

					// Check for Crouch
					animator.SetBool("isCrouching", Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D));
				} else {
					isJumping = true;
				}
				break;
		}

		// Handle Looking
		if (Input.GetKeyDown(KeyCode.W)) {
			direction = 0;
		}

		if (Input.GetKey(KeyCode.A) ) {
			direction = 3;
		}

		if (Input.GetKey(KeyCode.S)) {
			direction = 2;
		}

		if (Input.GetKey(KeyCode.D)) {
			direction = 1;
		}
	}

	public void JumpOver() {
		// Round
		transform.position = new Vector3((float)Math.Round(transform.position.x), 1, (float)Math.Round(transform.position.z));

		// Look
		model.transform.rotation = Quaternion.Euler(0, direction * 90, 0);

		// Repeat Jump
		if (repeatJump) {
			repeatJump = false;
			animator.SetBool("repeatJump", false);
		} else {
			// Stop Jumping
			isJumping = false;

			// Check for Crouch
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
				if (animator.GetBool("isCrouching")) {
					state = PlayerState.Crouch;
				}
			} else {
				state = PlayerState.Idle;
			}
		}
	}

	void SpawnCharacter()
	{
		// Spawn a Character Prefab
		int rand = Random.Range(0, models.Length);

		GameObject character = Instantiate(models[rand], transform.position, Quaternion.identity);
		character.transform.SetParent(model.transform);

		character.transform.localRotation = Quaternion.Euler(0, 90, 0);
	}
}
