using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
	[Header("Control")]
	public Direction direction;
	public float speed = 3f;

	void Start()
	{
		transform.localRotation = Quaternion.Euler(0, direction == Direction.Left ? 0 : 180, 0);
	}

	void Update()
	{
		transform.Translate(new Vector3(speed * (direction == Direction.Left ? -1 : 1) * Time.deltaTime, 0, 0), Space.World);
	}
}
