using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCondition : MonoBehaviour
{
    [SerializeField]
    GameObject victoryPanel;

    [SerializeField]
    LineRenderer bridgeLine;

    public static bool win = false;
    public static bool win2 = false;
    private GameLogic _gameLogic;

    private void Awake()
    {
        _gameLogic = FindObjectOfType<GameLogic>();
    }

    private void Update() {
        if(_gameLogic.currentIdOfAsteroid == _gameLogic.Asteroids.Count - 2 && win2) {
      //      win = true;
       //     Victory();
        }
    
    }

    private void Victory() {
        Debug.Log("You Won");
        win = false;
        // bridge.SetPosition(0, GameLogic.Asteroids[GameLogic.currentIdOfAsteroid].transform.position);
        //  bridge.SetPosition(1, GameLogic.Asteroids[GameLogic.Asteroids.Count - 1].transform.position);


        // victoryPanel.SetActive(true);

    }


}
