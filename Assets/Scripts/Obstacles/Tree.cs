using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
	[Header("Control")]
	public GameObject[] variants;

	void Start()
	{
		GameObject tree = Instantiate(variants[Random.Range(0, variants.Length)], transform.position, Quaternion.identity);
		tree.transform.SetParent(transform);
	}
}
