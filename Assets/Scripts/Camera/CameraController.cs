using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header("Control")]
	public GameObject target;

	[Space(10)]

	public Vector3 offset;
	public float damp = 0.25f;

	[Header("Debug")]
	private Vector3 velocity;

	void Start()
	{
		transform.position = new Vector3(Mathf.Clamp(target.transform.position.x * 0.5f, -1, 1), 1, target.transform.position.z) + offset;
	}

	void FixedUpdate()
	{
		Vector3 location = new Vector3(Mathf.Clamp(target.transform.position.x * 0.5f, -1, 1), 1, target.transform.position.z);
		transform.position = Vector3.SmoothDamp(transform.position, location + offset, ref velocity, damp);
	}
}