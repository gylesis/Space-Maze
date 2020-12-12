using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    Vector3 direction;
    [SerializeField]
    float speed = 50f;
    int damage;

    [SerializeField]
    AudioSource audio;

    bool isHostile;

    Rigidbody2D rb;

    GameObject spaceShipBullet;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audio.Play();
    }

    private void Update() {
        if (Vector3.Distance(transform.position, Player.Instance.transform.position) > 5) {
            if (audio)
                audio.volume -= 0.01f;
        }
        audio.volume = Mathf.Clamp((50 - (Vector3.Distance(transform.position, Player.Instance.transform.position) / 2)) / 100 - 0.4f, 0, 0.3f);
    }

    public void SetDirection(Vector3 _direction, int _damage, GameObject obj, bool isHostile)
    {
        this.isHostile = isHostile;
        direction = _direction;
        Destroy(gameObject, 5f);
        damage = _damage;

        Vector3 rotation = Vector3.zero;

        if (-direction.normalized.y >= 0)
        {
            rotation.z = 270 - Mathf.Asin(-direction.normalized.x) * Mathf.Rad2Deg;
        }
        else
        {
            rotation.z = 90 + Mathf.Asin(-direction.normalized.x) * Mathf.Rad2Deg;
        }

        spaceShipBullet = obj;
        transform.rotation = Quaternion.Euler(rotation);
    }

    void FixedUpdate()
    {
        transform.position += transform.right / speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Building" && isHostile)
        {
            Building building;
            if (!collision.TryGetComponent(out building))
              building = collision.transform.GetChild(0).GetComponent<Building>();
            building.TakeDamage(damage, spaceShipBullet);

            Instantiate(GameLogic.Instance.smallExplosion, transform.position + new Vector3(Random.value-.5f, Random.value-.5f),Quaternion.identity);
            Destroy(gameObject);
        }
        if (collision.tag == "Enemy" && !isHostile)
        {
            Enemy enemy;
            enemy = collision.transform.GetComponent<Enemy>();
            enemy.TakeDamage(damage);

            Instantiate(GameLogic.Instance.smallExplosion, transform.position + new Vector3(Random.value - .5f, Random.value - .5f), Quaternion.identity);
            Destroy(gameObject);
        }
    }


}
