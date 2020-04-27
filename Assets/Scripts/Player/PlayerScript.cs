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

	public bool isJumping;

	[Header("GameObjects")]
	public GameObject[] models;

	[Header("Animation")]
	private Animator animator;

	[Header("Input")]
	private bool vActive = false;
	private bool hActive = false;

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

		// Update Animation
		animator.SetBool("isJumping", isJumping);
	}

	void SpawnCharacter()
	{
		// Spawn a Character Prefab
		int rand = Random.Range(0, models.Length);

		GameObject character = Instantiate(models[rand], transform.position, Quaternion.identity);
		character.transform.SetParent(model.transform);

		character.transform.localRotation = Quaternion.Euler(0, 0, 0);
	}

	void HandleInput()
	{
		// Handle Input
		float vInput = Input.GetAxisRaw("Vertical");
		float hInput = Input.GetAxisRaw("Horizontal");

		// Vertical Pressed
		if (vInput != 0 && !vActive)
		{
			anyPressed(vInput);
			vPressed(vInput);

			vActive = true;
		}
		if (vInput == 0 && vActive)
		{
			anyReleased();
			vReleased();

			vActive = false;
		}

		// Horizontal Pressed
		if (hInput != 0 && !hActive)
		{
			anyPressed(hInput);
			hPressed(hInput);

			hActive = true;
		}
		if (hInput == 0 && hActive)
		{
			anyReleased();
			hReleased();

			hActive = false;
		}
	}

	void anyPressed(float input)
	{
		isJumping = true;
	}

	void anyReleased()
	{
		isJumping = false;
	}

	void vPressed(float input)
	{

	}

	void vReleased()
	{

	}

	void hPressed(float input)
	{

	}

	void hReleased()
	{

	}
}
