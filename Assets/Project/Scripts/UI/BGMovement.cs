using Project.Scripts.Game;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class BGMovement : MonoBehaviour
    {
        [SerializeField] Transform BG1;
        [SerializeField] Transform BG2;

        [SerializeField] float _speed = 0.01f;

        private float _distanceX;

        void Start()
        {
            _distanceX = BG2.position.x - BG1.position.x - 1;
        }

        void FixedUpdate()
        {
            Vector3 player = Player.Instance.transform.position;
            transform.position = new Vector3(transform.position.x, player.y, 10);

            float left = player.x - _distanceX;
            float right = player.x + _distanceX;

            BG1.Translate(Vector3.left * _speed);
            BG2.Translate(Vector3.left * _speed);

            if (BG1.position.x < left)
            {
                BG1.position = new Vector3(right, BG1.position.y, BG1.position.z);
            }

            if (BG2.position.x < left)
            {
                BG2.position = new Vector3(right, BG2.position.y, BG2.position.z);
            }

            if (BG1.position.x > right)
            {
                BG1.position = new Vector3(left, BG1.position.y, BG1.position.z);
            }

            if (BG2.position.x > right)
            {
                BG2.position = new Vector3(left, BG2.position.y, BG2.position.z);
            }
        }
    }
}