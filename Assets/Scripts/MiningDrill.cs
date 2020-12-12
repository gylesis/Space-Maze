using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningDrill : Building {
    int materialPerTick = 3;

    [SerializeField]
    float tickTime = 1f;

    int materialMined = 0;

    int maxAllowedToMine;

    bool allowToTick = true;

    [SerializeField]
    GameObject hpBar;

    private void OnEnable() {
       
    }

    private void Start() {
        name = "MiningDrill";
        hp = 200;
        maxHp = hp;
        GetComponent<Animator>().SetTrigger("Setup");

        switch (GameLogic.Asteroids[GameLogic.selectedAsteroid].GetComponent<Asteroid>().typeOfAsteroid) {

            case Asteroid.TypeOfAsteroid.enzima:
                idOfCurrentAsteroid = 1;
                break;

            case Asteroid.TypeOfAsteroid.chromium:
                idOfCurrentAsteroid = 2;
                break;

            case Asteroid.TypeOfAsteroid.linonium:
                idOfCurrentAsteroid = 3;
                break;
        }
    }

    protected override void Update() {
        if (allowToTick) {
            allowToTick = false;
            StartCoroutine(Income());
        }
        if(locatingAsteroid.oreAmount <= 0) {
            OnDeath();
        }

        hpBar.transform.localScale = new Vector3(hp / maxHp, hpBar.transform.localScale.y, hpBar.transform.localScale.z);
    }

    IEnumerator Income() {
        switch (idOfCurrentAsteroid) {
            case 1:
                GameLogic.enzimaAmount += materialPerTick;
                break;

            case 2:
                GameLogic.chromiumAmount += materialPerTick;
                break;

            case 3:
                GameLogic.linoniumAmount += materialPerTick;
                break;
        }
        locatingAsteroid.oreAmount -= materialPerTick;

        yield return new WaitForSeconds(tickTime);
        allowToTick = true;
    }

}
