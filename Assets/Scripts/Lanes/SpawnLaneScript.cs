using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLaneScript : LaneScript
{
	[Header("GameObjects")]
	public GameObject tree;

    override public void Populate(LaneType? type = null, LaneScript script = null) {
		GameObject tree1 = Instantiate(tree, transform.position + new Vector3(-4, 0, 0), Quaternion.identity);
		tree1.transform.SetParent(transform);

		GameObject tree2 = Instantiate(tree, transform.position + new Vector3(4, 0, 0), Quaternion.identity);
		tree2.transform.SetParent(transform);
	}
}
