using System;
using Project.Scripts.Game.Asteroids;
using Project.Scripts.Game.Buildings;
using UnityEngine;

namespace Project.Scripts.Game
{
    public class Player : MonoBehaviour
    {
        public static Player Instance;


        [SerializeField] private float _forceCoefficient = 1f;
        [SerializeField] private float speed = 1;
        [SerializeField] private float _allowedRadius = 3f;

        private Rigidbody2D _rigidbody;
        private CapsuleCollider2D collider;
        private Asteroid _asteroid;
        private float _timer;

        public void Construct()
        {
            
        }

        public void SetLocationAsteroid(Asteroid asteroid)
        {
            _asteroid = asteroid;
        }
        
        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            collider = GetComponent<CapsuleCollider2D>();
            Instance = this;
        }

        void FixedUpdate()
        {
            Move();
            AlignBySurface();
        }

        private void ApplyGravityOnPlayer()
        {
            var asteroidTransform = _asteroid.transform;
            
            Vector2 direction = transform.position - asteroidTransform.position;
            
            var distance = direction.magnitude;
            Vector2 force = (_forceCoefficient / distance * distance) * direction.normalized;

            if (direction.magnitude < _allowedRadius ) 
                _rigidbody.AddForce(force);
        }
        
        private void Update()
        {
            _timer += Time.deltaTime;
            ApplyGravityOnPlayer();
        }

        void Move()
        {
            float xInput = Input.GetAxis("Horizontal");
            transform.Translate(new Vector3(xInput, 0) * speed);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var asteroid = other.GetComponentInParent<Asteroid>();
            SetLocationAsteroid(asteroid); 
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Respawn") && Input.GetKey(KeyCode.Space) && _timer > 1)
            {
                _timer = 0;
                BridgeRender br = collision.gameObject.GetComponent<BridgeRender>();
                if ((br.pos1 - transform.position).magnitude < (br.pos2 - transform.position).magnitude)
                {
                    _rigidbody.AddForce((br.pos2 - br.pos1).normalized * 500);
                }
                else
                {
                    _rigidbody.AddForce((br.pos1 - br.pos2).normalized * 500);
                }
            }
        }

        void AlignBySurface()
        {
            if (_asteroid == null) return;
            RaycastHit2D rayToGround = Physics2D.Raycast(transform.position,
                _asteroid.transform.position - transform.position);
            Vector3 rotation = Vector3.zero;

            if (-rayToGround.normal.y >= 0)
            {
                rotation.z = 180 - Mathf.Asin(-rayToGround.normal.x) * 180 / Mathf.PI;
            }
            else
            {
                rotation.z = Mathf.Asin(-rayToGround.normal.x) * 180 / Mathf.PI;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), 0.25f);
        }

        private void OnDrawGizmos()
        {
            RaycastHit2D rayToGround = Physics2D.Raycast(transform.position, -transform.up);

            Debug.DrawLine(transform.position,
                transform.position + new Vector3(rayToGround.normal.x, rayToGround.normal.y));
        }
    }
}