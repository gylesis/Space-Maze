using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveState : StateMachineBehaviour
{    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.gameObject, 3f);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.GetComponent<Rigidbody2D>().AddForce((animator.GetComponent<Enemy>().targetPos - animator.transform.position) * -20);
    }

}
