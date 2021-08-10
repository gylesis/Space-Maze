using System;
using UniRx;
using UnityEngine;

namespace Project.Scripts.Game.Asteroids
{
    public class AsteroidsController : MonoBehaviour
    {
        [SerializeField] private AsteroidGenerator _asteroidGenerator;
        [SerializeField] private Camera _camera;
        private AsteroidCursorData _cursorData;

        public void Construct()
        {
            _cursorData = new AsteroidCursorData(_camera);
            _asteroidGenerator.Construct();
        }

        private void Update()
        {
            _cursorData.Raycasting();
        }
    }
}