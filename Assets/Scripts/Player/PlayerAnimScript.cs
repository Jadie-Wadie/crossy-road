using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimScript : StateMachineBehaviour
{
	private PlayerScript playerScript;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (stateInfo.IsName("Jump")) animator.GetComponent<PlayerScript>().jumpSpeed = stateInfo.length;
	}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		if (stateInfo.IsName("Jump")) animator.GetComponent<PlayerScript>().JumpOver();
    }
}
