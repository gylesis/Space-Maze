using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour {
    GameObject bulletPrefab;
    int damage;

    float timer = 0;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        damage = animator.gameObject.GetComponent<Enemy>().damage;
        bulletPrefab = animator.gameObject.GetComponent<Enemy>().bulletPrefab;
        animator.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        timer += Time.deltaTime;

        Debug.Log(animator.GetComponent<Enemy>().attackingBuilding);
        if(animator.GetComponent<Enemy>().attackingBuilding == null) {
            animator.SetTrigger("Leave");
        }

        if (timer >= 2) {
            SpawnBullet(animator);
            timer = 0;
        }
    }

    private void SpawnBullet(Animator animator) {
        var bullet = Instantiate(bulletPrefab, animator.transform.position, Quaternion.identity);
        bullet.GetComponent<BulletLogic>().SetDirection(animator.gameObject.GetComponent<Enemy>().targetPos - animator.transform.position, damage, animator.gameObject, true);
    }
}
