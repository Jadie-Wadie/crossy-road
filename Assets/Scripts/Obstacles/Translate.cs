using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour
{
	[Header("Control")]
	public float laneWidth;

	[Space(10)]

	public float speed = 3f;

	[Space(10)]

	public bool boost = false;
	public bool boosting = false;

	[Header("Debug")]
	public Renderer render;

	new private Transform transform;

	void Start()
	{
		transform = GetComponent<Transform>();

		render = transform.Find("default").GetComponent<Renderer>();
		transform.localRotation = Quaternion.Euler(0, Mathf.Clamp(speed, 0, 1) * 180, 0);
	}

	void Update()
	{
		transform.position += new Vector3((speed * (boosting ? 2 : 1)) * Time.deltaTime, 0, 0);

		// Check for Boost
		if (boost)
		{
			boosting = Mathf.Min(
				Mathf.Abs(render.bounds.center.x - render.bounds.size.x / 2),
				Mathf.Abs(render.bounds.center.x + render.bounds.size.x / 2)
				) > laneWidth / 2;
		}
	}
}
