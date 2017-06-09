using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    Player player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = Player.Instance;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.LaunchAttack();
        player.BounceOffBullet();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Apply damage value to enemies
        foreach (KeyValuePair<int, Collider2D> kvp in player.damagedEnemyList)
        {
            Debug.Log("Player: Applying damage to " + kvp.Key + " " + kvp.Value.name);
            Collider2D enemyCollider = kvp.Value;
            enemyCollider.gameObject.transform.parent.parent.GetComponent<Enemy>().TakeDamage(Player.Instance.damage);
        }
        player.incomingProjectileList.Clear();
        player.deflectedProjectileList.Clear();
        player.damagedEnemyList.Clear();
    }
}
