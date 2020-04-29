using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimScript : StateMachineBehaviour
{
	private PlayerScript playerScript;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		if (playerScript == null) playerScript = animator.GetComponent<PlayerScript>();

		if (stateInfo.IsName("Jump")) {
			playerScript.isJumping = true;
		}
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
		if (stateInfo.IsName("Jump")) {
			playerScript.isJumping = false;
			playerScript.JumpComplete();
		}
    }
}
