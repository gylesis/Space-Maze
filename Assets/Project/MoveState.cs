using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : StateMachineBehaviour {
    float forceSpeed;
    Vector3 targetPos;
    Rigidbody2D rb;
    float distanceToBuilding;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        distanceToBuilding = animator.gameObject.GetComponent<Enemy>().distanceToBuilding;
        forceSpeed = animator.gameObject.GetComponent<Enemy>().forceSpeed;
        targetPos = animator.gameObject.GetComponent<Enemy>().targetPos;
        rb = animator.gameObject.GetComponent<Enemy>().rb;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        distanceToBuilding = animator.gameObject.GetComponent<Enemy>().distanceToBuilding;
        RaycastHit2D rayToBuilding = Physics2D.Raycast(animator.transform.position, targetPos - animator.transform.position);

        /*   if (rayToBuilding.transform.tag != "Building") {
               //  rb.AddForce((targetPos - animator.transform.position) );
               animator.transform.Translate(targetPos - animator.transform.position,);
               rb.AddForce(Vector2.right * forceSpeed);
           }
           else if (rayToBuilding.transform.tag == "Building") {
               animator.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
               rb.AddForce((targetPos - animator.transform.position) * 1.5f);
           }
   */


        if (distanceToBuilding < 10f) {
            animator.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            animator.SetTrigger("AttackTarget");
        }

        else if (distanceToBuilding > 10.1f) {
            animator.SetTrigger("MoveToTarget");
            rb.AddForce((targetPos - animator.transform.position) / 0.5f);
        }

    }

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

}
