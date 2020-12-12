using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeVisual : MonoBehaviour
{
    [SerializeField]
    private LineRenderer bridgeLine;

    public static void BridgeCreate()
    {

        if (!CheckForAllowToBuild())
        {
            return;
        }
        else if (GameLogic.inMenu)
        {
            return;
        }

        var bridge = Instantiate(GameLogic.Instance.bridgePrefab);

        var startPoint = GameLogic.Asteroids[GameLogic.currentIdOfAsteroid].transform;
        var targetPoint = GameLogic.Asteroids[GameLogic.selectedAsteroid].transform;

        BridgeRender bridgeRenderer = bridge.GetComponent<BridgeRender>();
        bridgeRenderer.SetPoses(startPoint.position, targetPoint.position);

        GameLogic.bridges.Add(bridge);
        DefineBridgePoses();

        EdgeCollider2D collider = bridge.AddComponent<EdgeCollider2D>();
        collider.isTrigger = true;
        Vector2[] newVerts = {
            bridgeRenderer.pos1,
            bridgeRenderer.pos2
        };
        collider.points = newVerts;

        if(GameLogic.selectedAsteroid == GameLogic.Asteroids.Count - 2) {
            VictoryCondition.win = true;
            Debug.Log("You Won");
            CreateFinalBridge();

        }

    }

    public static void CreateFinalBridge() {
        var bridge = Instantiate(GameLogic.Instance.bridgePrefab);

        var startPoint = GameLogic.Asteroids[GameLogic.selectedAsteroid].transform;
        var targetPoint = GameLogic.Asteroids[GameLogic.Asteroids.Count - 1].transform;

        BridgeRender bridgeRenderer = bridge.GetComponent<BridgeRender>();
        bridgeRenderer.SetPoses(startPoint.position, targetPoint.position);

        GameLogic.bridges.Add(bridge);
        DefineBridgePoses();

        EdgeCollider2D collider = bridge.AddComponent<EdgeCollider2D>();
        collider.isTrigger = true;
        Vector2[] newVerts = {
            bridgeRenderer.pos1,
            bridgeRenderer.pos2
        };
        collider.points = newVerts;

        VictoryCondition.win = false;

    }


    public static void DefineBridgePoses()
    {
        for (int i = 0; i < GameLogic.bridges.Count; i++)
        {
            GameLogic.bridgePoses1.Add(GameLogic.bridges[i].transform.GetChild(0).position);
            GameLogic.bridgePoses2.Add(GameLogic.bridges[i].transform.GetChild(1).position);
        }
    }

    public static bool CheckForAllowToBuild()
    {
        if (Vector3.Distance(GameLogic.Asteroids[GameLogic.currentIdOfAsteroid].transform.position,
            GameLogic.Asteroids[GameLogic.selectedAsteroid].transform.position)
            > Builder.Instance.maxRangeForBridge)
        {
            return false;
        }
        else if (GameLogic.currentBuildingToBuild != 1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void OnMouseEnter()
    {
        if (CheckForAllowToBuild() && !GameLogic.inMenu)
        {

            if (gameObject.name == "Castle")
            {
                print("sdsadasd");
                bridgeLine.SetPosition(0, GameLogic.Asteroids[GameLogic.currentIdOfAsteroid].transform.position);
                bridgeLine.SetPosition(1, transform.position);
            }
            else
            {
                bridgeLine.SetPosition(0, GameLogic.Asteroids[GameLogic.currentIdOfAsteroid].transform.position);
                bridgeLine.SetPosition(1, GameLogic.Asteroids[GameLogic.selectedAsteroid].transform.position);
            }
        }
    }

    private void OnMouseExit()
    {
        bridgeLine.SetPosition(0, GameLogic.Asteroids[GameLogic.currentIdOfAsteroid].transform.position);
        bridgeLine.SetPosition(1, GameLogic.Asteroids[GameLogic.currentIdOfAsteroid].transform.position);
    }
}
