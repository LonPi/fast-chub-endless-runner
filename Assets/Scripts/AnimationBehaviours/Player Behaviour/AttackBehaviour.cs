using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour {

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.LaunchAttack();
        Player.Instance.BounceOffBullet();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        Dictionary<int, Collider2D> damagedEnemyList = Player.Instance.damagedEnemyList;
        // TODO: Apply damage value to enemies

        //foreach (KeyValuePair<int, GameObject> kvp in damagedEnemyList)
        //{
        //    Debug.Log("Applying damage to " + kvp.Key + " " + kvp.Value.name);
        //}
        
        damagedEnemyList.Clear();
    }
}
