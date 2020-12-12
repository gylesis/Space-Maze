using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidLevitation : MonoBehaviour {
    Vector3 startPos;

    Vector3 direction;

    float maxDeviation = 0.9f;

    private void Start() {
        startPos = transform.position;

        Calculate();
    }

    private void Update() {
        if (Vector3.Distance(transform.position, startPos + direction) < 0.3f) {
            Calculate();
            Debug.Log("calc");
        }
        Debug.Log("levi");

        transform.position = Vector3.Lerp(transform.position, startPos + direction, 0.008f);
    }


    private void Calculate() {
        float randomAngle = Random.value * 360;
        direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;
        direction *= Random.value * maxDeviation;
    }



}
