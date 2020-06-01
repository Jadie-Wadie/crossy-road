using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[Header("Control")]
	public GameObject target;

	[Space(10)]

	public bool shake = false;
	public float scale = 0.1f;

	[Space(10)]

	public Vector3 offset;
	public float damp = 0.25f;

	[Space(10)]

	public GameObject view;

	[Header("Debug")]
	private Vector3 velocity;

	void Awake()
	{
		view = transform.Find("View").gameObject;
	}

	void Start()
	{
		transform.position = new Vector3(Mathf.Clamp(target.transform.position.x * 0.5f, -1, 1), 1, target.transform.position.z) + offset;
	}

	void FixedUpdate()
	{
		// Lerp
		Vector3 location = new Vector3(Mathf.Clamp(target.transform.position.x * 0.5f, -1, 1), 1, target.transform.position.z);
		transform.position = Vector3.SmoothDamp(transform.position, location + offset, ref velocity, damp);

		// Shake
		if (shake)
		{
			view.transform.localPosition = new Vector3(Random.Range(-scale, scale), 0, 0);
		}
		else
		{
			view.transform.localPosition = new Vector3(0, 0, 0);
		}
	}
}