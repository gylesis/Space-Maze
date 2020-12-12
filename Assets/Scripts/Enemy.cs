using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private int hp = 50;

    public Vector3 targetPos;
    private float distanceToNearBuilding = 9999f;
    public float distanceToBuilding;

    [SerializeField]
    public GameObject bulletPrefab;

    [SerializeField]
    public int damage;

    [SerializeField]
    Animator animator;

    public GameObject attackingBuilding;

    [HideInInspector]
    public Rigidbody2D rb;

    [SerializeField]
    public float forceSpeed = 2f;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator.SetTrigger("FindTarget");
    }

    public void FindTarget() {
        var buildings = GameObject.FindGameObjectsWithTag("Building");

        foreach (var obj in buildings) {
            distanceToBuilding = Vector3.Distance(transform.position, obj.transform.position);

            if (distanceToBuilding < distanceToNearBuilding) {
                distanceToNearBuilding = distanceToBuilding;
            }
            targetPos = obj.transform.position;
            targetPos.y += 0.5f;
            attackingBuilding = obj;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position , targetPos);
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        Destroy(Instantiate(GameLogic.Instance.explosionPrefab, transform.position, Quaternion.identity), 2f);
        Destroy(gameObject);
    }

    public void RotateTowardsTarget() {

        RaycastHit2D rayToBuilding = Physics2D.Raycast(animator.transform.position, targetPos - transform.position);
        //  transform.rotation = Quaternion.FromToRotation(Vector3.up, rayToBuilding.normal);

        Vector3 rotation = Vector3.zero;

        if (-rayToBuilding.normal.y >= 0) {
            rotation.z = 180 - Mathf.Asin(-rayToBuilding.normal.x) * 180 / Mathf.PI;
        }
        else {
            rotation.z = Mathf.Asin(-rayToBuilding.normal.x) * 180 / Mathf.PI;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), 0.25f);
    }




    private void Update() {
        distanceToBuilding = Vector3.Distance(transform.position, targetPos);
        if (Input.GetKeyDown(KeyCode.G)) {
            GetComponent<Animator>().SetTrigger("FindTarget");
        }
        RotateTowardsTarget();
    }

}
