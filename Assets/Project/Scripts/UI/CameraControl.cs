using Project.Scripts.Game;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class CameraControl : MonoBehaviour
    { 
        private Camera _camera;
        private Transform _playerTransform;
        private const int zOffset = 10;

        [SerializeField] private float _rotationSpeed = 0.1f;
        [SerializeField] private float _moveSpeed = 0.1f;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            _playerTransform = FindObjectOfType<Player>().transform;
        }

        void FixedUpdate()
        {
            CorrectMovement();
            CorrectRotation();
        }

        private void CorrectMovement()
        {
            var playerPos = _playerTransform.position;

            Vector3 newPosition = new Vector3(playerPos.x, playerPos.y, playerPos.z - zOffset);
            transform.position = Vector3.Lerp(transform.position, newPosition, _moveSpeed);
        }

        private void CorrectRotation() => 
            transform.rotation = Quaternion.Lerp(transform.rotation, _playerTransform.rotation, _rotationSpeed);
    
    }
}