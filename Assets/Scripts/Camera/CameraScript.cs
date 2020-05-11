using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("Control")]
	public GameObject target;

	[Space(10)]

	public Vector3 offset;
	public float damp = 0.25f;

	[Header("Debug")]
    private Vector3 velocity;

	void Start() {
		transform.position = offset;
	}

    void FixedUpdate()
    {
		transform.position = Vector3.SmoothDamp(transform.position, new Vector3(0, 0, target.transform.position.z) + offset, ref velocity, damp);
    }
}
