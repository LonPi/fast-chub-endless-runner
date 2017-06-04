using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBehaviour : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.Jump = true;
        Player.Instance._rb2D.velocity = new Vector2(0f, Player.Instance.jumpSpeed);
        //Player.Instance._rb2D.AddForce(new Vector2(0f, Player.Instance.jumpSpeed));
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Player.Instance._rb2D.velocity.y <= 0)
        {
            animator.SetBool("land", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.Jump = false;
    }

}
