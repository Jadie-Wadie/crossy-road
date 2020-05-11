using System.Collections;
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
		// Validate
		if (bindings.Length < 2) throw new System.Exception("keybinds[] must have at least 2 entries");

		// Spawn Players
		int count = state == GameState.Singleplayer ? 1 : 2;
		players = new GameObject[count];

		for (int i = 0; i < count; i++) {
			// Spawn the Player
			GameObject player = Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
			player.transform.SetParent(playerParent);
			players[i] = player;

			// Set Player Keybinds
			player.GetComponent<PlayerScript>().keybinds = bindings[i % bindings.Length];

			// Spawn the Camera
			GameObject camera = Instantiate(cameraPrefab, player.transform.position + cameraOffset, cameraPrefab.transform.localRotation);
			camera.transform.SetParent(cameraParent);
			
			// Set Camera Properties
			CameraScript cameraScript = camera.GetComponent<CameraScript>();
			cameraScript.target = player;
			cameraScript.offset = cameraOffset;

			camera.GetComponent<Camera>().rect = new Rect((float)i / count, 0, 1f / count, 1);

			// Spawn a Camera Line
			if (i < count - 1) {
				GameObject line = Instantiate(cameraLinePrefab, new Vector3(lineParent.rect.width * (i + 1) / (float)count, 0, 0), Quaternion.identity);
				line.transform.SetParent(lineParent, false);
			}
		}
	}
}
