using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
	[Header("Control")]
	public float speed = 3f;

	void Start()
	{
		transform.localRotation = Quaternion.Euler(0, Mathf.Clamp(speed, 0, 1) * 180, 0);
	}

	void Update()
	{
		transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0), Space.World);
	}
}
