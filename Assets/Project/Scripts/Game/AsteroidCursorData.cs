using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts.Game.Asteroids
{
    [Serializable]
    public class AsteroidCursorData
    {
        public Subject<Asteroid> OnAsteroidEnter = new Subject<Asteroid>();
        public Subject<Asteroid> OnAsteroidDrag = new Subject<Asteroid>();

        private readonly Asteroid _asteroid;

        private Camera _camera;

        public AsteroidCursorData(Camera camera)
        {
            _camera = camera;
        }

        public void Raycasting()
        {
            RaycastHit2D hit;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            hit = Physics2D.Raycast(ray.origin, new Vector2(0, 0));

            if (hit.collider) 
                Debug.Log(hit.transform, hit.transform);
        }

        public void OnDrag(PointerEventData eventData) =>
            OnAsteroidDrag.OnNext(_asteroid);

        public void OnPointerExit(PointerEventData eventData) =>
            OnAsteroidEnter.OnNext(null);
    }
}