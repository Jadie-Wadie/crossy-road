using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LaneVariant {
	public Material matA;
	public Material matB;
}

public class LaneScript : MonoBehaviour
{
	[Header("Control")]
	public GameObject lEnd;
	public GameObject rEnd;

	[Space(10)]

	public LaneVariant[] variants;

	[Space(10)]

	public PopulateScript script;

	[HideInInspector]
	public GameObject[] objects;

	// Set Lane Variant
	public void SetVariant(int index) {
		LaneVariant variant = variants[index - 1];

		GetComponent<MeshRenderer>().material = variant.matA;

		lEnd.GetComponent<MeshRenderer>().material = variant.matB;
		rEnd.GetComponent<MeshRenderer>().material = variant.matB;
	}

	// Populate Objects
    public void Populate(LaneType? type = null, LaneScript laneScript = null) {
		if (script != null) script.Populate(type, laneScript);
	}
}
