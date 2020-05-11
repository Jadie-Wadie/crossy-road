﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
	Singleplayer, Multiplayer
}

public class GameScript : MonoBehaviour
{
	[Header("Control")]
	public GameState state;

	[Space(10)]

	public GameObject playerPrefab;
	public Transform playerParent;

	[Space(10)]

	public GameObject cameraPrefab;
	public Transform cameraParent;

	public Vector3 cameraOffset = new Vector3(0.75f, 10f, -6f);

	[Space(10)]

	public GameObject cameraLinePrefab;
	public RectTransform lineParent;

	[Header("Input")]
	public Keybinds[] bindings;

	[Header("Debug")]
	private GameObject[] players;

    void Start()
    {
        SetupGame();
    }

    void Update()
    {
        
    }

	// Spawn Players and Cameras
	void SetupGame() {
		GameObject SpawnPlayer(Keybinds keybinds, int id = 0, int max = 1) {
			// Spawn the Player
			GameObject player = Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
			player.transform.SetParent(playerParent);

			// Set Player Keybinds
			player.GetComponent<PlayerScript>().keybinds = keybinds;

			// Spawn the Camera
			GameObject camera = Instantiate(cameraPrefab, player.transform.position + cameraOffset, cameraPrefab.transform.localRotation);
			camera.transform.SetParent(cameraParent);
			
			// Set Camera Properties
			CameraScript cameraScript = camera.GetComponent<CameraScript>();
			cameraScript.target = player;
			cameraScript.offset = cameraOffset;

			camera.GetComponent<Camera>().rect = new Rect((float)id / max, 0, 1f / max, 1);

			// Spawn a Camera Line
			if (id < max - 1) {
				GameObject line = Instantiate(cameraLinePrefab, new Vector3(lineParent.rect.width * (id + 1) / (float)max, 0, 0), Quaternion.identity);
				line.transform.SetParent(lineParent, false);
			}

			return player;
		}

		// Validate
		if (bindings.Length < 2) throw new System.Exception("keybinds[] must have at least 2 entries");

		// Handle Gamemode
		switch(state) {
			case GameState.Singleplayer: 
				players = new GameObject[1];

				players[0] = SpawnPlayer(bindings[0]);
				break;
			case GameState.Multiplayer:
				players = new GameObject[2];

				players[0] = SpawnPlayer(bindings[0], 0, 2);
				players[1] = SpawnPlayer(bindings[1], 1, 2);
				break;
		}
	}
}
