using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	public enum Gamemode
	{
		Singleplayer, Multiplayer
	}

	[Header("Control")]
	public bool isPlaying = false;
	public Gamemode gamemode;

	[Space(10)]

	public int alivePlayers;

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

	[Header("UI")]
	public Button playButton;
	public Button modeButton;

	[Space(10)]

	public Image black;

	[Space(10)]

	public PlayerUI player1;
	public PlayerUI player2;

	[Space(10)]

	public Text highscore;

	[System.Serializable]
	public struct PlayerUI
	{
		public Text distance;
		public Text gameOver;
	}

	[Header("Input")]
	public Keybinds[] inputs;

	[Header("Debug")]
	private WorldGenerator worldGenerator;

	void Awake()
	{
		worldGenerator = GetComponent<WorldGenerator>();
	}

	void Start()
	{
		SetupGame();
		SetupUI();
	}

	void Update()
	{
		if (isPlaying)
		{
			switch (gamemode)
			{
				case Gamemode.Singleplayer:
					// Update UI
					player1.distance.text = Mathf.Max(Mathf.RoundToInt(players[0].transform.position.z), 0).ToString();
					break;

				case Gamemode.Multiplayer:
					// Update UI
					player1.distance.text = Mathf.Max(Mathf.RoundToInt(players[0].transform.position.z), 0).ToString();
					player2.distance.text = Mathf.Max(Mathf.RoundToInt(players[1].transform.position.z), 0).ToString();
					break;
			}
		}
	}

	// Setup Game
	void SetupGame()
	{
		// Spawn Players
		SpawnPlayers();

		// Regenerate World
		worldGenerator.Generate();
	}

	// Setup UI
	void SetupUI()
	{
		if (isPlaying)
		{
			// Menu
			playButton.gameObject.SetActive(false);
			modeButton.gameObject.SetActive(false);

			// HUD
			switch (gamemode)
			{
				case Gamemode.Singleplayer:
					player1.distance.enabled = true;
					player2.distance.enabled = false;

					highscore.enabled = true;
					break;

				case Gamemode.Multiplayer:
					player1.distance.enabled = true;
					player2.distance.enabled = true;

					highscore.enabled = false;
					break;
			}

			player1.gameOver.enabled = false;
			player2.gameOver.enabled = false;
		}
		else
		{
			// Show Menu
			playButton.gameObject.SetActive(true);
			modeButton.gameObject.SetActive(true);
		}
	}

	// Start Game
	public void StartGame()
	{
		// Start Playing
		isPlaying = true;

		// Setup UI
		SetupUI();
	}

	// Toggle Gamemode
	public void ToggleGamemode()
	{
		// Toggle GameMode
		gamemode = gamemode == Gamemode.Singleplayer ? Gamemode.Multiplayer : Gamemode.Singleplayer;

		// Update UI
		modeButton.transform.Find("Text").GetComponent<Text>().text = gamemode == Gamemode.Singleplayer ? "1" : "2";

		// Start Animation
		black.GetComponent<Animator>().SetBool("isVisible", true);
	}

	// Spawn Players
	void SpawnPlayers()
	{
		// Remove Players
		foreach (Transform child in playerParent.transform)
		{
			GameObject.Destroy(child.gameObject);
		}

		foreach (Transform child in cameraParent.transform)
		{
			GameObject.Destroy(child.gameObject);
		}

		// Validate
		if (inputs.Length < 2) throw new System.Exception("keybinds[] must have at least 2 entries");

		// Spawn Players
		players = new GameObject[gamemode == Gamemode.Singleplayer ? 1 : 2];
		switch (gamemode)
		{
			case Gamemode.Singleplayer:
				// Player
				players[0] = SpawnPlayer(0, new Vector3(0, 1, 0), inputs[0]);
				SpawnCamera(players[0], new Rect(0, 0, 1, 1), 35f);

				// Camera Line
				cameraSplit.SetActive(false);
				break;

			case Gamemode.Multiplayer:
				// Player 1
				players[0] = SpawnPlayer(0, new Vector3(-1, 1, 0), inputs[0]);
				SpawnCamera(players[0], new Rect(0, 0, 0.5f, 1), 40f);

				// Player 2
				players[1] = SpawnPlayer(1, new Vector3(0, 1, 0), inputs[1]);
				SpawnCamera(players[1], new Rect(0.5f, 0, 0.5f, 1), 40f);

				// Camera Line
				cameraSplit.SetActive(true);
				break;
		}

		alivePlayers = players.Length;
	}

	// Spawn a Player
	GameObject SpawnPlayer(int id, Vector3 position, Keybinds keybinds)
	{
		GameObject player = Instantiate(playerPrefab, position, Quaternion.identity);
		player.transform.SetParent(playerParent);

		Player script = player.GetComponent<Player>();
		script.keybinds = keybinds;
		script.playerID = id;

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

	// Player Died
	public void PlayerDied(int player)
	{
		if (player == 0)
		{
			player1.gameOver.enabled = true;
			player1.gameOver.text = player1.distance.text;
		}

		if (player == 1)
		{
			player2.gameOver.enabled = true;
			player2.gameOver.text = player2.distance.text;
		}

		if (--alivePlayers == 0) GameOver();
	}

	// Game Over
	void GameOver()
	{
		Debug.Log("Game Over");

		switch (gamemode)
		{
			case Gamemode.Singleplayer:
				// Save Highscore
				break;

			case Gamemode.Multiplayer:
				// Show winner
				break;
		}

		// Show Menu
		SetupUI();
	}
}
