using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Game.Enemy;
using UnityEngine;

public class StartState : StateMachineBehaviour {
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.gameObject.GetComponent<Enemy>().FindTarget();


        if (animator.gameObject.GetComponent<Enemy>().targetPos != Vector3.zero) {
            animator.SetTrigger("MoveToTarget");
        }
    }
}
