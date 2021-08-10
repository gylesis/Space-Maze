using System;
using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Utils;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Project.Scripts.Game.Asteroids
{
    [Serializable]
    public class AsteroidGenerator
    {
        [SerializeField] private Asteroid[] _asteroidsToSpawn;
        [SerializeField] private GameObject _planet;
        [SerializeField] private GameObject _parent;
        [SerializeField] private int _locationSize;
        [SerializeField] private float _rangeOfSpawn;

        private const string AsteroidDatasPath = "Data/AsteroidDatas";
        private AsteroidData GetRandomData => _asteroidDatas.GetRandom();

        private List<AsteroidData> _asteroidDatas = new List<AsteroidData>();
        private List<Asteroid> _spawnedAsteroids = new List<Asteroid>();


        public void Construct()
        {
            _asteroidDatas = Resources.LoadAll<AsteroidData>(AsteroidDatasPath).ToList();

            Generate();
        }

        public void Generate()
        {
            var mainSpawnedAsteroid = InstantiateAsteroid(Vector3.zero);

            Asteroid tempMain = null;

            for (int i = 0; i <= _locationSize; i++)
            {
                int amountOfNewAsteroids = 1;
                for (int j = 0; j <= amountOfNewAsteroids; j++)
                {
                    Vector3 position = Vector3.zero;
                    bool isCapableToSpawn = false;

                    while (!isCapableToSpawn)
                    {
                        float randomAngle = Random.value * 360;
                        Vector3 direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;
                        position = mainSpawnedAsteroid.transform.position +
                                   direction * (_rangeOfSpawn + Random.value * 5f);

                        isCapableToSpawn = true;
                        foreach (var asteroid in _spawnedAsteroids)
                        {
                            if ((asteroid.transform.position - position).magnitude < _rangeOfSpawn)
                            {
                                isCapableToSpawn = false;
                            }
                        }
                    }

                    var asteroidInstance = InstantiateAsteroid(position);

                    if (asteroidInstance.transform.position.x > mainSpawnedAsteroid.transform.position.x)
                    {
                        tempMain = asteroidInstance;
                    }
                }

                if (tempMain != null) mainSpawnedAsteroid = tempMain;
            }

          //  Vector3 planetPosition = mainSpawnedAsteroid.transform.position + Vector3.right * _rangeOfSpawn * Mathf.PI;
           // Object.Instantiate(_planet, planetPosition, Quaternion.identity);
            
            AsteroidsSort();
        }

        private Asteroid InstantiateAsteroid(Vector3 position)
        {
            var randomCounter = (int) Mathf.Round(Random.value * (_asteroidsToSpawn.Length - 1));

            var prefab = _asteroidsToSpawn[randomCounter];

            var asteroidInstance = Object.Instantiate(prefab, position, Quaternion.identity);

            var asteroidData = GetRandomData;

            asteroidInstance.Construct(asteroidData);

            // asteroidInstance.OnTriggerEnter.Subscribe();

            asteroidInstance.transform.parent = _parent.transform;

            _spawnedAsteroids.Add(asteroidInstance);
            return asteroidInstance;
        }

        private void AsteroidsSort()
        {
            for (int j = 0; j < _spawnedAsteroids.Count - 1; j++)
            {
                for (int i = 0; i < _spawnedAsteroids.Count - 1; i++)
                {
                    if (_spawnedAsteroids[i].transform.position.x > _spawnedAsteroids[i + 1].transform.position.x)
                    {
                        var temp = _spawnedAsteroids[i];
                        _spawnedAsteroids[i] = _spawnedAsteroids[i + 1];
                        _spawnedAsteroids[i + 1] = temp;
                    }
                }
            }
        }
    }
}