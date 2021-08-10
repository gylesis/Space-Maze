using System;
using Project.Scripts.Game.Asteroids;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Project.Scripts.Game.Buildings
{
    [Serializable]
    public class BlueprintCreator 
    {
        private GameObject _blueprintToView;
        public BlueprintBuildData BuildData { get; private set; }
        public BuildingData BuildingData { get; private set; }
        
        private IDisposable _showBluePrint;

        public void Construct(Subject<Asteroid> onAsteroidEnter, Subject<BuildingData> onBuildingPick)
        {
            onBuildingPick
                .Subscribe(SetBuilding);

            onAsteroidEnter
                .Subscribe(ShowBlueprint);
            BuildData = new BlueprintBuildData();
        }

        private void SetBuilding(BuildingData buildingData) => 
            BuildingData = buildingData;

        public void ShowBlueprint(Asteroid asteroid)
        {
            if (asteroid == null)
            {
                DeleteBlueprint();
                _showBluePrint?.Dispose();
                return;
            }

            Debug.Log(asteroid);
            BuildData.LocationAsteroid = asteroid;
            InstantiatePrefab();
            
            _showBluePrint = Observable.EveryUpdate().Subscribe(Show);
        }

        private void InstantiatePrefab()
        {
            var prefab = BuildingData.Blueprint;
            _blueprintToView = Object.Instantiate(prefab);
        }

        private void Show(long _)
        {
            var hit = GetRaycastInfoToSurface();

            var position = hit.point;
            var rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            
            SetTransform(position, rotation);
        }

        private void SetTransform(Vector2 position, Quaternion rotation)
        {
            _blueprintToView.transform.position = position;
            _blueprintToView.transform.rotation = rotation;

            BuildData.Rotation = rotation;
            BuildData.Position = position;
        }

        private RaycastHit2D GetRaycastInfoToSurface()
        {
            var mousePos = Input.mousePosition;
            var rayDirection = mousePos;
            rayDirection.z += 5;

            var ray = new Ray2D(mousePos, rayDirection);

            var hit = Physics2D.Raycast(ray.origin, ray.direction);

            var asteroid = hit.collider.GetComponent<Asteroid>();
            var centerOfASteroid = asteroid.transform.position;

            var directionToAsteroid = (centerOfASteroid - mousePos).normalized;

            var rayToSurface = Physics2D.Raycast(mousePos, directionToAsteroid * 10);

            return rayToSurface;
        }

        public void DeleteBlueprint() => 
            Object.Destroy(_blueprintToView);

    }

    public class BlueprintBuildData
    {
        public Asteroid LocationAsteroid { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        
    }
}