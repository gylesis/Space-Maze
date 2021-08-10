using System.Collections.Generic;
using Project.Scripts.Game.Asteroids;
using Project.Scripts.UI;
using UniRx;
using UnityEngine;

namespace Project.Scripts.Game.Buildings
{
    public class Builder : MonoBehaviour
    {
        [SerializeField] public float _maxRangeForBridge = 100f;
        [SerializeField] private CurrentBuildingVisualiser _buildingVisualiser;
        private BlueprintCreator _blueprintCreator;

        private GameLogic _gameLogic;
        private readonly Subject<BuildingData> OnClick = new Subject<BuildingData>();
        private Subject<BuildingData> OnBuildingPick = new Subject<BuildingData>();

        private BuildingData _building;

        private void Awake()
        {
            _gameLogic = FindObjectOfType<GameLogic>();
        }

        public void Construct(ItemSelector[] itemSelector, Subject<BuildingData> onBuildingPick,
            BlueprintCreator blueprintCreator)
        {
            OnBuildingPick = onBuildingPick;
            
            OnClick
                .TakeUntilDestroy(this)
                .Subscribe(SetBuildingToBuild);

            foreach (var selector in itemSelector)
                selector.Construct(OnClick);

            _buildingVisualiser.Construct(OnClick);

            _blueprintCreator = blueprintCreator;
        }

        private void SetBuildingToBuild(BuildingData building)
        {
            Debug.Log(building.name);
            OnBuildingPick.OnNext(building);
            _building = building;
        }

        public void Build()
        {
            var prefab = _building.Prefab;
            var position = _blueprintCreator.BuildData.Position;
            var rotation = _blueprintCreator.BuildData.Rotation;

            var instance = Instantiate(prefab, position, rotation);
            
            instance.Construct(_blueprintCreator.BuildData.LocationAsteroid);

            /*
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
                        _building = Instantiate(_buildings[_gameLogic.currentBuildingToBuild], BlueprintCreator.Position,
                            BlueprintCreator.Rotation, Asteroid.CurrentAsteroid.transform);
                        _building.GetComponent<Building>().LocatingAsteroid = Asteroid.CurrentAsteroid;
                    }
                }
                else if (_gameLogic.currentBuildingToBuild == 2)
                {
                    if (_gameLogic.SpendResources(GameLogic.Instance.turretEnzimaPrice,
                        _gameLogic.turretChromiumPrice, GameLogic.Instance.turretLimoniumPrice))
                    {
                        _building = Instantiate(_buildings[_gameLogic.currentBuildingToBuild], BlueprintCreator.Position,
                            BlueprintCreator.Rotation, Asteroid.CurrentAsteroid.transform);
                    }
                }
            }*/
        }

        public bool CheckForAllowToBuildBridge()
        {
            /*if (Vector3.Distance(_gameLogic.Asteroids[_gameLogic.currentIdOfAsteroid].transform.position,
                    _gameLogic.Asteroids[_gameLogic.selectedAsteroid].transform.position)
                > _maxRangeForBridge)
            {
                return false;
            }

            return true;*/
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
}