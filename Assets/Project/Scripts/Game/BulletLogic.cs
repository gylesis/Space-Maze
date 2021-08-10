using Project.Scripts.Game.Buildings;
using UnityEngine;

namespace Project.Scripts.Game
{
    public class BulletLogic : MonoBehaviour
    {
        [SerializeField] private float _speed = 50f;
        [SerializeField] AudioSource audio;

        private int _damage;
        private bool _isHostile;
        private Vector3 _direction;
        private Rigidbody2D _rb;
        private GameObject _spaceShipBullet;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            audio.Play();
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, Player.Instance.transform.position) > 5)
            {
                if (audio)
                    audio.volume -= 0.01f;
            }

            audio.volume =
                Mathf.Clamp(
                    (50 - (Vector3.Distance(transform.position, Player.Instance.transform.position) / 2)) / 100 - 0.4f, 0,
                    0.3f);
        }

        public void SetDirection(Vector3 _direction, int _damage, GameObject obj, bool isHostile)
        {
            this._isHostile = isHostile;
            this._direction = _direction;
            Destroy(gameObject, 5f);
            this._damage = _damage;

            Vector3 rotation = Vector3.zero;

            if (-this._direction.normalized.y >= 0)
            {
                rotation.z = 270 - Mathf.Asin(-this._direction.normalized.x) * Mathf.Rad2Deg;
            }
            else
            {
                rotation.z = 90 + Mathf.Asin(-this._direction.normalized.x) * Mathf.Rad2Deg;
            }

            _spaceShipBullet = obj;
            transform.rotation = Quaternion.Euler(rotation);
        }

        void FixedUpdate()
        {
            transform.position += transform.right / _speed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Building" && _isHostile)
            {
                Building building;
                if (!collision.TryGetComponent(out building))
                    building = collision.transform.GetChild(0).GetComponent<Building>();
             //   building.TakeDamage(_damage, _spaceShipBullet);

                Instantiate(GameLogic.Instance.smallExplosion,
                    transform.position + new Vector3(Random.value - .5f, Random.value - .5f), Quaternion.identity);
                Destroy(gameObject);
            }

            if (collision.tag == "Enemy" && !_isHostile)
            {
                Enemy.Enemy enemy;
                enemy = collision.transform.GetComponent<Enemy.Enemy>();
                enemy.TakeDamage(_damage);

                Instantiate(GameLogic.Instance.smallExplosion,
                    transform.position + new Vector3(Random.value - .5f, Random.value - .5f), Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}