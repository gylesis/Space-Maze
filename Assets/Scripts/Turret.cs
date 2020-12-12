using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Building {
    [SerializeField]
    float agreRange;

    [SerializeField]
    GameObject Bullet;

    [SerializeField]
    Transform SpawnPoint;

    Vector3 direction;
    Collider2D collider;

    [SerializeField]
    GameObject hpBar;


    [SerializeField]
    int damage;
    float timer;
    bool isShooting;



    void Start() {
        hp = 100;
        maxHp = hp;

        GetComponent<CircleCollider2D>().radius = agreRange;
    }

    private void Update() {
        timer += Time.deltaTime;
        if (collider == null) return;
        direction = collider.transform.position - transform.GetChild(0).position;
        if (isShooting) {
            if (timer > 3) {
                Shoot(direction);
                timer = 0;
            }
        }

        hpBar.transform.localScale = new Vector3(hp / maxHp, hpBar.transform.localScale.y, hpBar.transform.localScale.z);

    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            collider = collision;
            Rotate(direction);

            if (direction == null) return;
            RaycastHit2D hit = Physics2D.Raycast(transform.GetChild(0).position, direction);
            Debug.Log(hit.collider.name);
            if (hit.collider.CompareTag("Enemy")) {
                isShooting = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            isShooting = false;
        }
    }

    void Rotate(Vector3 direction) {
        Vector3 rotation = Vector3.zero;

        if (-direction.normalized.y >= 0) {
            rotation.z = 180 - Mathf.Asin(-direction.normalized.x) * Mathf.Rad2Deg;
        }
        else {
            rotation.z = Mathf.Asin(-direction.normalized.x) * Mathf.Rad2Deg;
        }

        transform.GetChild(0).rotation = Quaternion.Lerp(transform.GetChild(0).rotation, Quaternion.Euler(rotation), 0.1f);
    }

    void Shoot(Vector3 direction) {
        var bullet = Instantiate(Bullet, SpawnPoint.position, Quaternion.identity);

        bullet.GetComponent<BulletLogic>().SetDirection(direction, damage, null, false);

    }

}
