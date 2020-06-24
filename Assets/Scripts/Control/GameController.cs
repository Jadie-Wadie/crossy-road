using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	public enum GameState
	{
		Menu, Play, Over
	}

	public enum GameMode
	{
		Singleplayer, Multiplayer
	}

	[Header("Control")]
	public GameState gameState;
	public GameMode gameMode;

	[Space(10)]

	public int alivePlayers;

	[Space(10)]

	public int highScore = 0;

	[Header("Player")]

	public GameObject playerPrefab;
	public Transform playerParent;

	[Space(10)]

	[HideInInspector]
	public GameObject[] players;
	[HideInInspector]
	public GameObject[] cameras;

	[Header("Camera")]

	public GameObject cameraPrefab;
	public Transform cameraParent;

	[Space(10)]

	public Vector3 cameraOffset = new Vector3(0.75f, 10f, -6f);

	[Header("UI")]
	public Canvas canvas;

	[Space(10)]

	public GameObject gamePanel;
	public GameObject menuPanel;

	[Space(10)]

	public Button playButton;
	public Button modeButton;

	private bool modeEnabled = true;

	[Space(10)]

	public Image black;
	public float fadeLength;

	[Space(10)]

	public PlayerUI player1;
	public PlayerUI player2;

	[Space(10)]

	public Text highScoreText;

	[Space(10)]

	public Text frameText;

	private float frameTime = 0f;
	private List<float> frameTimes = new List<float>();

	[System.Serializable]
	public struct PlayerUI
	{
		public Text distance;
		public Text gameOver;
		public Image crown;
	}

	[Header("Input")]
	public Keybinds[] inputs;

	[Header("Debug")]
	private WorldGenerator worldGenerator;

	void Awake()
	{
		worldGenerator = GetComponent<WorldGenerator>();
		highScore = PlayerPrefs.GetInt("highscore", 0);
	}

	void Start()
	{
		SetupGame();
		SetupUI();
	}

	void Update()
	{
		if (gameState == GameState.Play)
		{
			switch (gameMode)
			{
				case GameMode.Singleplayer:
					// Update UI
					int distance = Mathf.Max(Mathf.RoundToInt(players[0].transform.position.z), 0);
					player1.distance.text = distance.ToString();

					highScore = Mathf.Max(distance, highScore);
					highScoreText.text = highScore.ToString();
					break;

				case GameMode.Multiplayer:
					// Update UI
					player1.distance.text = Mathf.Max(Mathf.RoundToInt(players[0].transform.position.z), 0).ToString();
					player2.distance.text = Mathf.Max(Mathf.RoundToInt(players[1].transform.position.z), 0).ToString();
					break;
			}
		}

		// Show FPS
		frameTimes.Add(Time.deltaTime);

		if (frameTime < Time.time - 0.25f)
		{
			frameText.text = $"{Mathf.RoundToInt(1f / frameTimes.Average()).ToString().PadLeft(3, '0')}";

			frameTimes = new List<float>();
			frameTime = Time.time;
		}
	}

	void FixedUpdate()
	{
		PlayerPrefs.SetInt("highscore", highScore);
		PlayerPrefs.Save();
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
		switch (gameState)
		{
			case GameState.Menu:
				menuPanel.SetActive(true);
				gamePanel.SetActive(false);
				break;

			case GameState.Play:
				menuPanel.SetActive(false);
				gamePanel.SetActive(true);

				switch (gameMode)
				{
					case GameMode.Singleplayer:
						player1.distance.enabled = true;
						player2.distance.enabled = false;

						highScoreText.enabled = true;
						break;

					case GameMode.Multiplayer:
						player1.distance.enabled = true;
						player2.distance.enabled = true;

						highScoreText.enabled = false;
						break;
				}

				player1.gameOver.enabled = false;
				player2.gameOver.enabled = false;

				player1.crown.enabled = false;
				player2.crown.enabled = false;
				break;

			case GameState.Over:
				menuPanel.SetActive(true);
				gamePanel.SetActive(true);
				break;
		}

		// Move GameOver Text
		Rect rect = (canvas.transform as RectTransform).rect;
		player1.gameOver.transform.position = new Vector3(rect.width * (gameMode == GameMode.Singleplayer ? 0.5f : 0.25f), rect.height * 0.5f, 0);

		// Gamemode Button Text
		modeButton.transform.Find("Text").GetComponent<Text>().text = gameMode == GameMode.Singleplayer ? "1" : "2";
	}

	// Play Button
	public void PlayButton()
	{
		StartCoroutine(StartGame());
	}

	// Start Game
	IEnumerator StartGame()
	{
		if (gameState == GameState.Menu)
		{
			// Start Playing
			gameState = GameState.Play;

			// Enable Movement
			foreach (GameObject player in players)
			{
				player.GetComponent<Player>().playing = true;
			}

			// Setup UI
			SetupUI();
		}
		else
		{
			// UI
			Animator animator = black.GetComponent<Animator>();
			animator.SetBool("isVisible", true);

			// Delay
			yield return new WaitForSeconds(fadeLength);

			// State
			gameState = GameState.Play;

			// Regenerate Game
			SetupGame();
			SetupUI();

			// Enable Movement
			foreach (GameObject player in players)
			{
				player.GetComponent<Player>().playing = true;
			}

			// UI
			animator.SetBool("isVisible", false);
		}
	}

	// Game Mode Button
	public void GameModeButton()
	{
		StartCoroutine(ToggleGamemode());
	}

	// Toggle Game Mode
	IEnumerator ToggleGamemode()
	{
		// Disable Button
		if (!modeEnabled) yield break;
		modeEnabled = false;

		// Toggle
		gameMode = gameMode == GameMode.Singleplayer ? GameMode.Multiplayer : GameMode.Singleplayer;
		gameState = GameState.Menu;

		// UI
		Animator animator = black.GetComponent<Animator>();
		animator.SetBool("isVisible", true);

		// Delay
		yield return new WaitForSeconds(fadeLength);

		// UI
		animator.SetBool("isVisible", false);

		// Regenerate Game
		SetupGame();
		SetupUI();

		// Delay
		yield return new WaitForSeconds(fadeLength);

		// Enable Button
		modeEnabled = true;
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
		players = new GameObject[gameMode == GameMode.Singleplayer ? 1 : 2];
		cameras = new GameObject[players.Length];

		switch (gameMode)
		{
			case GameMode.Singleplayer:
				// Player
				players[0] = SpawnPlayer(0, new Vector3(0.5f, 1, 0), inputs[0]);
				cameras[0] = SpawnCamera(players[0], new Rect(0, 0, 1, 1), 35f);
				break;

			case GameMode.Multiplayer:
				// Player 1
				players[0] = SpawnPlayer(0, new Vector3(-0.5f, 1, 0), inputs[0]);
				cameras[0] = SpawnCamera(players[0], new Rect(0, 0, 0.5f, 1), 40f);

				// Player 2
				players[1] = SpawnPlayer(1, new Vector3(0.5f, 1, 0), inputs[1]);
				cameras[1] = SpawnCamera(players[1], new Rect(0.5f, 0, 0.5f, 1), 40f);
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
		script.id = id;

		script.playing = false;

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

		cameraScript.view.GetComponent<Camera>().rect = rect;
		cameraScript.view.GetComponent<Camera>().fieldOfView = fieldOfView;

		return camera;
	}

	// Player Died
	public IEnumerator PlayerDied(int player)
	{
		if (player == 0)
		{
			player1.gameOver.text = player1.distance.text;
			player1.gameOver.enabled = true;
		}

		if (player == 1)
		{
			player2.gameOver.text = player2.distance.text;
			player2.gameOver.enabled = true;
		}

		if (--alivePlayers == 0) GameOver();

		cameras[player].GetComponent<CameraController>().shake = true;

		yield return new WaitForSeconds(0.25f);

		cameras[player].GetComponent<CameraController>().shake = false;
	}

	// Game Over
	void GameOver()
	{
		gameState = GameState.Over;

		// Show Multiplayer Winner
		if (gameMode == GameMode.Multiplayer)
		{
			// Draw
			if (int.Parse(player1.gameOver.text) == int.Parse(player2.gameOver.text))
			{
				player1.crown.enabled = true;
				player2.crown.enabled = true;
			}
			else if (int.Parse(player1.gameOver.text) > int.Parse(player2.gameOver.text))
			{
				player1.crown.enabled = true;
				player2.crown.enabled = false;
			}
			else
			{
				player1.crown.enabled = false;
				player2.crown.enabled = true;
			}
		}

		// Show Menu
		SetupUI();
	}
}
