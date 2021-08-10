using UnityEngine;

namespace Project.Scripts.Game.Asteroids
{
    public class AsteroidLevitation : MonoBehaviour
    {
        private Vector3 _startPos;

        private Vector3 _direction;

        private float _maxDeviation = 0.9f;

        private void Start()
        {
            _startPos = transform.position;

            Calculate();
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, _startPos + _direction) < 0.3f)
            {
                Calculate();
                //   Debug.Log("calc");
            }

            // Debug.Log("levi");

            transform.position = Vector3.Lerp(transform.position, _startPos + _direction, 0.008f);
        }


        private void Calculate()
        {
            float randomAngle = Random.value * 360;
            _direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;
            _direction *= Random.value * _maxDeviation;
        }
    }
}