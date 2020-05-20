using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Lane : MonoBehaviour
{
	[System.Serializable]
	public struct Variant
	{
		public Material main;
		public Material ends;
	}

	[Header("Control")]
	public static int laneWidth = 8;

	[Header("GameObjects")]
	public GameObject cube;

	[Space(10)]

	[HideInInspector]
	public GameObject[] objects = new GameObject[laneWidth];

	[Space(10)]

	[HideInInspector]
	public GameObject main;
	[HideInInspector]
	public GameObject lEnd;
	[HideInInspector]
	public GameObject rEnd;

	[Header("Variants")]
	public Variant[] variants;

	public void Awake()
	{
		SpawnObjects();
	}

	// Spawn the Lane Objects
	public void SpawnObjects()
	{
		// Spawn Center
		main = Instantiate(cube, this.transform.position, Quaternion.identity);
		main.transform.SetParent(transform);

		main.name = "Main";
		main.transform.localScale = new Vector3(laneWidth, 1, 1);

		// Spawn Left
		lEnd = Instantiate(cube, this.transform.position - new Vector3(laneWidth, 0, 0), Quaternion.identity);
		lEnd.transform.SetParent(transform);

		lEnd.name = "lEnd";
		lEnd.transform.localScale = new Vector3(laneWidth, 1, 1);

		// Spawn Right
		rEnd = Instantiate(cube, this.transform.position + new Vector3(laneWidth, 0, 0), Quaternion.identity);
		rEnd.transform.SetParent(transform);

		rEnd.name = "rEnd";
		rEnd.transform.localScale = new Vector3(laneWidth, 1, 1);
	}

	// Set the Lane Variant
	public void SetVariant(int i)
	{
		Variant variant = variants[i];
		main.GetComponent<MeshRenderer>().material = variant.main;
		lEnd.GetComponent<MeshRenderer>().material = variant.ends;
		rEnd.GetComponent<MeshRenderer>().material = variant.ends;
	}

	// Populate Lane
	public abstract void Populate(Lane prevLane);

	/*
	switch (prevLane)
	{
		case GrassLane grass:
			Debug.Log("Grass");
			break;

		case SpawnLane spawn:
			Debug.Log("Spawn");
			break;

		default:
			break;
	}
	*/
}
