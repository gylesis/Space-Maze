using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningDrill : Building
{
    [SerializeField] float tickTime = 1f;
    [SerializeField] private GameObject hpBar;

    private int materialMined = 0;
    private int maxAllowedToMine;
    private int materialPerTick = 3;
    private bool allowToTick = true;
    private GameLogic _gameLogic;

    private void Awake()
    {
        _gameLogic = FindObjectOfType<GameLogic>();
        
        name = "MiningDrill";
        _hp = 200;
        _maxHp = _hp;
        GetComponent<Animator>().SetTrigger("Setup");

        switch (_gameLogic.Asteroids[_gameLogic.selectedAsteroid].GetComponent<Asteroid>().typeOfAsteroid)
        {
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

    protected override void Update()
    {
        if (allowToTick)
        {
            allowToTick = false;
            StartCoroutine(Income());
        }

        if (LocatingAsteroid.oreAmount <= 0)
        {
            OnDeath();
        }

        hpBar.transform.localScale =
            new Vector3(_hp / _maxHp, hpBar.transform.localScale.y, hpBar.transform.localScale.z);
    }

    IEnumerator Income()
    {
        switch (idOfCurrentAsteroid)
        {
            case 1:
                _gameLogic.enzimaAmount.Value += materialPerTick;
                break;

            case 2:
                _gameLogic.chromiumAmount.Value += materialPerTick;
                break;

            case 3:
                _gameLogic.linoniumAmount.Value += materialPerTick;
                break;
        }

        LocatingAsteroid.oreAmount -= materialPerTick;

        yield return new WaitForSeconds(tickTime);
        allowToTick = true;
    }
}