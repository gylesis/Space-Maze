using System;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintCreator : MonoBehaviour
{
    public static BlueprintCreator Instance;

    [SerializeField] List<GameObject> _blueprints = new List<GameObject>();
    
    public GameObject BlueprintToView;

    public static Vector3 posToBuild;

    public static Quaternion rotationToBuild;
    private GameLogic _gameLogic;

    private void Awake()
    {
        Instance = this;
        _gameLogic = FindObjectOfType<GameLogic>();
    }

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        Vector3 mousePos = MousePosition.GetMouseWorldPosition();
        Vector3 centerOfAsteroid = _gameLogic.Asteroids[_gameLogic.selectedAsteroid].transform.position;


        Vector3 direction = (mousePos - centerOfAsteroid).normalized;

        RaycastHit2D rayToSurface = Physics2D.Raycast(centerOfAsteroid + direction * 3f, -direction);

        posToBuild = rayToSurface.point;

        BlueprintToView.transform.position = posToBuild;
        rotationToBuild = Quaternion.FromToRotation(Vector3.up, rayToSurface.normal);

        BlueprintToView.transform.rotation = rotationToBuild;
    }

    public void ShowBlueprint()
    {
        if (!Builder.Instance.CheckForAllowToBuild())
        {
            return;
        }
        else if (!_gameLogic.isStateToCreate)
        {
            return;
        }

        BlueprintToView = _blueprints[_gameLogic.currentBuildingToBuild];
        BlueprintToView = Instantiate(BlueprintToView);
    }

    public void DeleteBluepint()
    {
        if (!Builder.Instance.CheckForAllowToBuild())
        {
            return;
        }
        else if (!_gameLogic.isStateToCreate)
        {
            return;
        }

        Destroy(BlueprintToView);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(posToBuild, 1);
    }
}