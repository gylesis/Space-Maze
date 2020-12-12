using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Range
{
    float leftValue;
    float rightValue;

    static bool isOverlap(Range range1, Range range2) =>
        (range1.rightValue > range2.leftValue || range1.leftValue < range2.rightValue ||
         range1.leftValue > range2.leftValue && range1.rightValue < range2.rightValue ||
         range2.leftValue > range1.leftValue && range2.rightValue < range1.rightValue);

}

public class AsteroidGeneration : MonoBehaviour
{
    [SerializeField]
    int k = 2;
    [SerializeField]
    int l = 2;

    [SerializeField]
    int locationSize;
    [SerializeField]
    GameObject[] asteroids;
    [SerializeField]
    GameObject Planet;
    [SerializeField]
    float rangeOfSpawn;
    [SerializeField]
    GameObject Parent;
    static GameObject mainSpawnedAsteroid;


    void Start()
    {
        List<GameObject> newAsteroids = new List<GameObject>();
        mainSpawnedAsteroid = Instantiate(asteroids[(int)Mathf.Round(Random.value * (asteroids.Length - 1))], Vector3.zero, Quaternion.identity);
        newAsteroids.Add(mainSpawnedAsteroid);
        GameObject tempObj = null;
        GameObject tempMain = null;

        for (int i = 0; i <= locationSize; i++)
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
                    position = mainSpawnedAsteroid.transform.position + direction * (rangeOfSpawn + Random.value * 5f);

                    isCapableToSpawn = true;
                    foreach (GameObject asteroid in newAsteroids)
                    {
                        if ((asteroid.transform.position - position).magnitude < rangeOfSpawn)
                        {
                            isCapableToSpawn = false;
                        }
                    }
                }

                tempObj = Instantiate(asteroids[(int)Mathf.Round(Random.value * (asteroids.Length - 1))], position, Quaternion.identity);
                tempObj.transform.parent = Parent.transform;
                newAsteroids.Add(tempObj);

                if (tempObj.transform.position.x > mainSpawnedAsteroid.transform.position.x)
                {
                    tempMain = tempObj;
                }
            }
            if (tempMain != null) mainSpawnedAsteroid = tempMain;
        } 

        Vector3 PlanetPosition = mainSpawnedAsteroid.transform.position + Vector3.right * rangeOfSpawn * Mathf.PI;
        Instantiate(Planet, PlanetPosition, Quaternion.identity);
    }
}
