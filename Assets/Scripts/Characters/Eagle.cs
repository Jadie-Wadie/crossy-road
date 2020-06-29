using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
	[Header("Control")]
	public float speed = 2f;

	[Space(10)]

	public GameObject target;

	void Update()
	{
		transform.position = new Vector3(target.transform.position.x, transform.position.y, transform.position.z - speed * Time.deltaTime);
	}
}
