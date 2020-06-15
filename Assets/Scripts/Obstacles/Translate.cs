using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour
{
	[Header("Control")]
	public float speed = 3f;
	public float boost = 0f;

	[Space(10)]

	public bool boosting = true;

	void Start()
	{
		transform.localRotation = Quaternion.Euler(0, Mathf.Clamp(speed, 0, 1) * 180, 0);
	}

	void Update()
	{
		transform.Translate(new Vector3((speed + (boosting ? boost * (speed < 0 ? -1 : 1) : 0)) * Time.deltaTime, 0, 0), Space.World);
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Not Walkable"))
		{
			boosting = !boosting;
		}
	}
}
