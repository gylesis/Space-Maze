using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    GameObject Enemy;

    float CD = 20;
    [SerializeField]
    float time;
    float timer;

    [SerializeField]
    float rangeOfSpawn = 40;

    private void Start()
    {
       // Time.timeScale = 5;
    }

    void Update()
    {
        time = Time.time;
        if (Time.time > CD)
        {
            timer += Time.deltaTime;
            if (timer > CD)
            {
                if (GameObject.FindGameObjectsWithTag("Building").Length == 0) return;
                SpawnEnemies();
                if(CD >= 30) CD -=.5f;
                timer = 0;
            }
        }
    }

    void SpawnEnemies()
    {
        float randomAngle = Random.value * 360;
        Vector3 direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;
        Vector3 position = direction * rangeOfSpawn;

        Instantiate(Enemy, position + new Vector3(Random.value + 8, Random.value + 8), Quaternion.identity);
    }
}
