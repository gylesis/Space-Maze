using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Game.Buildings
{
    public class Turret : Building
    {
        [SerializeField] private float _agreRange = 20f;

        [SerializeField] private GameObject Bullet;

        [SerializeField] private Transform _spawnPoint;

        private Vector3 _direction;
        private Collider2D _collider;

        [SerializeField] private GameObject _hpBar;

        [SerializeField] private int _damage = 20;
        private float _timer;
        private bool _isShooting;

        void Awake()
        {
            GetComponent<CircleCollider2D>().radius = _agreRange;
            
            var slider = GetComponentInChildren<Slider>();
            var healthBar = new HealthBar(slider);

            _buildingData.OnHeatlhChanged.Subscribe(healthBar.UpdateBar);
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_collider == null) return;
            _direction = _collider.transform.position - transform.GetChild(0).position;
            if (_isShooting)
            {
                if (_timer > 3)
                {
                    Shoot(_direction);
                    _timer = 0;
                }
            }
        
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                _collider = collision;
                Rotate(_direction);

                if (_direction == null) return;
                RaycastHit2D hit = Physics2D.Raycast(transform.GetChild(0).position, _direction);
                Debug.Log(hit.collider.name);
                if (hit.collider.CompareTag("Enemy"))
                {
                    _isShooting = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                _isShooting = false;
            }
        }

        void Rotate(Vector3 direction)
        {
            Vector3 rotation = Vector3.zero;

            if (-direction.normalized.y >= 0)
            {
                rotation.z = 180 - Mathf.Asin(-direction.normalized.x) * Mathf.Rad2Deg;
            }
            else
            {
                rotation.z = Mathf.Asin(-direction.normalized.x) * Mathf.Rad2Deg;
            }

            transform.GetChild(0).rotation =
                Quaternion.Lerp(transform.GetChild(0).rotation, Quaternion.Euler(rotation), 0.1f);
        }

        void Shoot(Vector3 direction)
        {
            var bullet = Instantiate(Bullet, _spawnPoint.position, Quaternion.identity);

            bullet.GetComponent<BulletLogic>().SetDirection(direction, _damage, null, false);
        }
    
    }
}