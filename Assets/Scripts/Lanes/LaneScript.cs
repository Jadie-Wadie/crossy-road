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
	public LaneVariant[] variants;

	[Space(10)]

	public GameObject[] objects;

	[Header("Debug")]
	public MeshRenderer meshRenderer;

	// Set Lane Variant
	public void SetVariant(int index) {
		if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();

		LaneVariant variant = variants[index - 1];
		meshRenderer.material = variant.materialA;
	}

	// Populate Objects
    public virtual void Populate(LaneType? type = null, LaneScript script = null) {
		Debug.LogWarning($"{this.GetType()}.Populate() is not overridden");
	}
}
