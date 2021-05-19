using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public static Builder Instance;

    [SerializeField] public List<GameObject> _buildings = new List<GameObject>();

    [SerializeField] public float _maxRangeForBridge = 100f;

    [SerializeField] private GameObject _drillPreviewPrefab;

    [SerializeField] private GameObject _building;
    private GameLogic _gameLogic;

    private void Awake()
    {
        Instance = this;
        _gameLogic = FindObjectOfType<GameLogic>();
    }

    public void Build()
    {
        if (GameLogic.inMenu)
        {
            return;
        }

        if (_gameLogic.currentBuildingToBuild == 1)
        {
            if (CheckForAllowToBuildBridge())
            {
                if (_gameLogic.SpendResources(0, 0, GameLogic.Instance.bridgeLimoniumPrice))
                {
                    BridgeVisual.BridgeCreate();
                }
            }
        }
        else if (CheckForAllowToBuild() && _gameLogic.isStateToCreate)
        {
            if (_gameLogic.currentBuildingToBuild == 0)
            {
                if (_gameLogic.SpendResources(_gameLogic.drillEnzimaPrice, _gameLogic.drillChromiumPrice,
                    GameLogic.Instance.drillLimoniumPrice))
                {
                    _building = Instantiate(_buildings[_gameLogic.currentBuildingToBuild], BlueprintCreator.posToBuild,
                        BlueprintCreator.rotationToBuild, Asteroid.CurrentAsteroid.transform);
                    _building.GetComponent<Building>().LocatingAsteroid = Asteroid.CurrentAsteroid;
                }
            }
            else if (_gameLogic.currentBuildingToBuild == 2)
            {
                if (_gameLogic.SpendResources(GameLogic.Instance.turretEnzimaPrice,
                    _gameLogic.turretChromiumPrice, GameLogic.Instance.turretLimoniumPrice))
                {
                    _building = Instantiate(_buildings[_gameLogic.currentBuildingToBuild], BlueprintCreator.posToBuild,
                        BlueprintCreator.rotationToBuild, Asteroid.CurrentAsteroid.transform);
                }
            }
        }
    }

    public bool CheckForAllowToBuildBridge()
    {
        if (Vector3.Distance(_gameLogic.Asteroids[_gameLogic.currentIdOfAsteroid].transform.position,
                _gameLogic.Asteroids[_gameLogic.selectedAsteroid].transform.position)
            > _maxRangeForBridge)
        {
            return false;
        }

        return true;
    }

    public bool CheckForAllowToBuild()
    {
        if (_gameLogic.selectedAsteroid == _gameLogic.currentIdOfAsteroid)
        {
            return true;
        }

        return false;
    }
}