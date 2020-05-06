using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
	[Header("Control")]
	public GameObject playerPrefab;
	public Transform playerParent;

	[Space(10)]

	public GameObject[] players;

	

	[Header("Debug")]
	private int playerCount = 2;

    void Start()
    {
		// Spawn Players
        SpawnPlayers();
    }

    void Update()
    {
        
    }

	void SpawnPlayers() {
		players = new GameObject[playerCount];

		int laneWidth = 9;
		for (int i = 0; i < playerCount; i++) {
			players[i] = Instantiate(playerPrefab, new Vector3(laneWidth / (playerCount + 1) * (i + 1) - Mathf.Ceil(laneWidth / 2f), 1, 0), Quaternion.identity);
			players[i].transform.SetParent(playerParent);
		}

		Camera.main.GetComponent<CameraScript>().targets = players;
	}
}
