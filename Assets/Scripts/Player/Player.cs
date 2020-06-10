using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public struct Keybinds
{
	public KeyCode W;
	public KeyCode A;
	public KeyCode S;
	public KeyCode D;
}

public class Player : MonoBehaviour
{
	public enum PlayerState
	{
		Idle,
		Crouch,
		Jump,
		Dead
	}

	[Header("Control")]
	public bool playing = true;
	public int playerID;

	[Space(10)]

	public GameObject model;

	[Space(10)]

	public PlayerState state = PlayerState.Idle;
	public bool repeatJump = false;

	[Space(10)]

	public int direction = 0;
	public bool canMove = true;

	[Space(10)]

	public bool isSticking = false;
	public GameObject stickObject;
	public Vector3 stickPos;

	[Header("Input")]
	public Keybinds keybinds;

	[Header("GameObjects")]
	public GameObject[] models;

	[Header("Animation")]
	public float jumpSpeed;
	private Animator animator;

	[Header("Debug")]
	private GameController gameController;

	void Start()
	{
		model = transform.Find("Model").gameObject;
		animator = GetComponent<Animator>();

		gameController = GameObject.Find("GameController").GetComponent<GameController>();

		SpawnCharacter();
	}

	void Update()
	{
		// Handle State
		if (playing)
		{
			switch (state)
			{
				case PlayerState.Idle:
					// Update Animator
					animator.SetBool("isCrouching", false);

					// Check for Crouch
					if (Input.GetKey(keybinds.W) || Input.GetKey(keybinds.A) || Input.GetKey(keybinds.S) || Input.GetKey(keybinds.D))
					{
						state = PlayerState.Crouch;
					}

					// Look
					model.transform.rotation = Quaternion.Euler(0, direction * 90, 0);
					break;

				case PlayerState.Crouch:
					// Update Animator
					animator.SetBool("isCrouching", true);

					// Check for Jump
					if (Input.GetKeyUp(keybinds.W) || Input.GetKeyUp(keybinds.A) || Input.GetKeyUp(keybinds.S) || Input.GetKeyUp(keybinds.D))
					{
						if (!Input.GetKey(keybinds.W) && !Input.GetKey(keybinds.A) && !Input.GetKey(keybinds.S) && !Input.GetKey(keybinds.D))
						{
							state = PlayerState.Jump;
							animator.SetTrigger("shouldJump");

							CheckMovement();
						}
					}

					// Look
					model.transform.rotation = Quaternion.Euler(0, direction * 90, 0);
					break;

				case PlayerState.Jump:
					// Update Animator
					animator.SetBool("isCrouching", false);

					// Move
					if (canMove) transform.Translate(model.transform.forward * (1 / jumpSpeed) * Time.deltaTime);

					// Check for Chaining
					if (Input.GetKeyUp(keybinds.W) || Input.GetKeyUp(keybinds.A) || Input.GetKeyUp(keybinds.S) || Input.GetKeyUp(keybinds.D))
					{
						repeatJump = true;
						animator.SetTrigger("shouldJump");
					}

					// Check for Crouch
					animator.SetBool("isCrouching", Input.GetKey(keybinds.W) || Input.GetKey(keybinds.A) || Input.GetKey(keybinds.S) || Input.GetKey(keybinds.D));
					break;
			}

			// Handle Looking
			if (Input.GetKey(keybinds.W)) direction = 0;
			if (Input.GetKey(keybinds.A)) direction = 3;
			if (Input.GetKey(keybinds.S)) direction = 2;
			if (Input.GetKey(keybinds.D)) direction = 1;
		}
		else
		{
			// Dead
			if (state == PlayerState.Dead)
			{
				// Stick to the Vehicle
				if (isSticking)
				{
					transform.rotation = Quaternion.Euler(0, direction * 90, 0);
					model.transform.localRotation = Quaternion.Euler(0, 0, 0);

					if (stickObject != null) transform.position = stickObject.transform.position + stickPos;

					Debug.DrawLine(transform.position, transform.position - new Vector3(stickPos.x, 0, stickPos.z).normalized);
				}

			}
		}
	}

	// Jump Animation Complete
	public void JumpOver()
	{
		if (state != PlayerState.Dead)
		{
			// Round Position and Update Direction
			transform.position = new Vector3((float)Math.Round(transform.position.x), 1, (float)Math.Round(transform.position.z));
			model.transform.rotation = Quaternion.Euler(0, direction * 90, 0);

			// Repeat Jump
			if (repeatJump)
			{
				repeatJump = false;
				CheckMovement();
			}
			else
			{
				// Check for Crouch
				state = animator.GetBool("isCrouching") ? PlayerState.Crouch : PlayerState.Idle;
			}
		}
	}

	// Check for Obstacle
	void CheckMovement()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, model.transform.TransformDirection(Vector3.forward), out hit, 1))
		{
			switch (hit.transform.tag)
			{
				case "Walkable":
				case "Vehicle":
					canMove = true;
					break;

				case "Not Walkable":
					canMove = false;
					break;
			}
		}
		else
		{
			canMove = true;
		}
	}

	// Spawn a Character Prefab
	void SpawnCharacter()
	{
		int rand = Random.Range(0, models.Length);

		GameObject character = Instantiate(models[rand], transform.position, Quaternion.identity);
		character.transform.SetParent(model.transform);

		character.transform.localRotation = Quaternion.Euler(0, 90, 0);
	}

	void OnTriggerEnter(Collider other)
	{
		if (playing)
		{
			if (other.gameObject.CompareTag("Vehicle"))
			{
				if (state == PlayerState.Jump)
				{
					stickPos = transform.position - other.gameObject.transform.position;

					Vector2 norm = new Vector2(stickPos.x, stickPos.y).normalized;
					float angle = Vector2.Angle(Vector2.up, norm);

					if (35f < angle && angle < 135f)
					{
						animator.SetTrigger("shouldFlat");
					}
					else
					{
						isSticking = true;
						animator.SetTrigger("shouldSplat");
						stickObject = other.gameObject;
					}
				}
				else
				{
					animator.SetTrigger("shouldFlat");
				}

				state = PlayerState.Dead;
				playing = false;

				StartCoroutine(gameController.PlayerDied(playerID));
			}
		}
	}
}
