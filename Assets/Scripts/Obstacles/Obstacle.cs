using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	[Header("Control")]
	public bool rotate = false;

	[Space(10)]

	public GameObject[] variants;

	void Start()
	{
		GameObject obstacle = Instantiate(variants[Random.Range(0, variants.Length)], transform.position, Quaternion.identity);
		obstacle.transform.SetParent(transform);

		if (rotate) obstacle.transform.localRotation = Quaternion.Euler(0, 90 * Random.Range(0, 4), 0);
	}
}
