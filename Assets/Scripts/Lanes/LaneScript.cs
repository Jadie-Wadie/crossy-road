using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LaneVariant {
	public Material materialA;
	public Material materialB;
}

public class LaneScript : MonoBehaviour
{
	[Header("Control")]
	public GameObject leftEnd;
	public GameObject rightEnd;

	[Space(10)]

	public LaneVariant[] variants;

	[Space(10)]

	public PopulateScript script;

	[HideInInspector]
	public GameObject[] objects;

	// Set Lane Variant
	public void SetVariant(int index) {
		LaneVariant variant = variants[index - 1];

		GetComponent<MeshRenderer>().material = variant.materialA;

		leftEnd.GetComponent<MeshRenderer>().material = variant.materialB;
		rightEnd.GetComponent<MeshRenderer>().material = variant.materialB;
	}

	// Populate Objects
    public void Populate(LaneType? type = null, LaneScript laneScript = null) {
		if (script != null) script.Populate(type, laneScript);
	}
}
