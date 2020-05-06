using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("Control")]
	public GameObject[] targets;

	[Space(10)]

	public float damp = 0.2f;
	public Vector3 offset = new Vector3(2.5f, 8f, -6.5f);

	[Header("Debug")]
    private Vector3 velocity;

	void Start() {
		transform.position = offset;
	}

    void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(0, 0, 0);

		for (int i = 0; i < targets.Length; i++) {
			targetPos.z += targets[i].transform.position.z;
		}
		if (targets.Length != 0) targetPos.z /= targets.Length;

		transform.position = Vector3.SmoothDamp(transform.position, targetPos + offset, ref velocity, damp);
    }
}
