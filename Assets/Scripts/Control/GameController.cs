using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameController : MonoBehaviour
{
	public enum Gamemode
	{
		Singleplayer, Multiplayer
	}

	[Header("Control")]
	public Gamemode gamemode;

	[Header("Player")]

	public GameObject playerPrefab;
	public Transform playerParent;

	[Space(10)]

	[HideInInspector]
	public GameObject[] players;

	[Header("Camera")]

	public GameObject cameraPrefab;
	public Transform cameraParent;

	[Space(10)]

	public Vector3 cameraOffset = new Vector3(0.75f, 10f, -6f);
	public GameObject cameraSplit;

	[Header("Input")]
	public Keybinds[] inputs;

	void Start()
	{
		SetupGame();
	}

	void Update()
	{

	}

	// Setup the Game
	void SetupGame()
	{
		// Validate
		if (inputs.Length < 2) throw new System.Exception("keybinds[] must have at least 2 entries");

		// Spawn Players
		players = new GameObject[gamemode == Gamemode.Singleplayer ? 1 : 2];
		switch (gamemode)
		{
			case Gamemode.Singleplayer:
				// Player
				players[0] = SpawnPlayer(new Vector3(0, 1, 0), inputs[0]);
				SpawnCamera(players[0], new Rect(0, 0, 1, 1), 35f);

				// Camera Line
				cameraSplit.SetActive(false);
				break;
			case Gamemode.Multiplayer:
				// Player 1
				players[0] = SpawnPlayer(new Vector3(-1, 1, 0), inputs[0]);
				SpawnCamera(players[0], new Rect(0, 0, 0.5f, 1), 40f);

				// Player 2
				players[1] = SpawnPlayer(new Vector3(0, 1, 0), inputs[1]);
				SpawnCamera(players[1], new Rect(0.5f, 0, 0.5f, 1), 40f);

				// Camera Line
				cameraSplit.SetActive(true);
				break;
		}
	}

	// Spawn a Player
	GameObject SpawnPlayer(Vector3 position, Keybinds keybinds)
	{
		GameObject player = Instantiate(playerPrefab, position, Quaternion.identity); // new Vector3(0, 1, 0)
		player.transform.SetParent(playerParent);

		player.GetComponent<Player>().keybinds = keybinds; // bindings[i % bindings.Length];

		return player;
	}

	// Spawn a Camera
	GameObject SpawnCamera(GameObject target, Rect rect, float fieldOfView)
	{
		GameObject camera = Instantiate(cameraPrefab, new Vector3(0, 0, 0), cameraPrefab.transform.localRotation);
		camera.transform.SetParent(cameraParent);

		// Set Camera Properties
		CameraController cameraScript = camera.GetComponent<CameraController>();
		cameraScript.target = target;
		cameraScript.offset = cameraOffset;

		camera.GetComponent<Camera>().rect = rect;
		camera.GetComponent<Camera>().fieldOfView = fieldOfView;

		return camera;
	}
}
