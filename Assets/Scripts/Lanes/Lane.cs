using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LaneVariant
{
	public Material matA;
	public Material matB;
}

[System.Serializable]
public struct SpawnedLane
{
	public GameObject gameObject;

}

public class Lane : MonoBehaviour
{
	[Header("Control")]
	public LaneVariant[] variants;

	[Space(10)]

	public static int laneWidth = 8;

	[Header("GameObjects")]
	private GameObject lEnd;
	private GameObject rEnd;

	[Space(10)]

	public GameObject[] objects;

	void Awake()
	{
		lEnd = transform.Find("lEnd").gameObject;
		rEnd = transform.Find("rEnd").gameObject;
	}

	// Set Lane Variant
	public void SetVariant(int index)
	{
		LaneVariant variant = variants[index - 1];

		GetComponent<MeshRenderer>().material = variant.matA;

		lEnd.GetComponent<MeshRenderer>().material = variant.matB;
		rEnd.GetComponent<MeshRenderer>().material = variant.matB;
	}

	// Populate Lane
	public virtual void Populate(SpawnedLane prevLane)
	{
		Debug.Log($"{this.GetType()}.Populate()");
	}
}
