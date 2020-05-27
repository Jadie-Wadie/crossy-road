using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackAnimation : StateMachineBehaviour
{
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (stateInfo.IsName("Fade In")) Debug.Log("Fade Update");
	}
}
