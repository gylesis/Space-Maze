using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour {

    public static Builder Instance;

    [SerializeField]
    public List<GameObject> buildings = new List<GameObject>();

    [SerializeField]
    public float maxRangeForBridge = 100f;


    [SerializeField]
    GameObject drillPreviewPrefab;

    [SerializeField]
    GameObject building;

    private void Awake() {
        Instance = this;
    }

    public void Build() {
        if (GameLogic.inMenu) {
            return;
        }
        if (GameLogic.currentBuildingToBuild == 1) {
            if (CheckForAllowToBuildBridge()) {
                if (GameLogic.SpendResources(0, 0, GameLogic.Instance.bridgeLimoniumPrice)) {
                    BridgeVisual.BridgeCreate();
                }
            }

        }
        else if (CheckForAllowToBuild() && GameLogic.isStateToCreate) {

            if (GameLogic.currentBuildingToBuild == 0) {
                if (GameLogic.SpendResources(GameLogic.Instance.drillEnzimaPrice, GameLogic.Instance.drillChromiumPrice, GameLogic.Instance.drillLimoniumPrice)) {
                    building = Instantiate(buildings[GameLogic.currentBuildingToBuild], BlueprintCreator.posToBuild, BlueprintCreator.rotationToBuild, Asteroid.CurrentAsteroid.transform);
                    building.GetComponent<Building>().locatingAsteroid = Asteroid.CurrentAsteroid;
                }

            }
            else if (GameLogic.currentBuildingToBuild == 2) {
                if (GameLogic.SpendResources(GameLogic.Instance.turretEnzimaPrice, GameLogic.Instance.turretChromiumPrice, GameLogic.Instance.turretLimoniumPrice)) {
                    building = Instantiate(buildings[GameLogic.currentBuildingToBuild], BlueprintCreator.posToBuild, BlueprintCreator.rotationToBuild, Asteroid.CurrentAsteroid.transform);
                }
            }
        }
    }

    public bool CheckForAllowToBuildBridge() {
        if (Vector3.Distance(GameLogic.Asteroids[GameLogic.currentIdOfAsteroid].transform.position,
            GameLogic.Asteroids[GameLogic.selectedAsteroid].transform.position)
            > maxRangeForBridge) {
            return false;
        }
        return true;
    }

    public bool CheckForAllowToBuild() {
        if (GameLogic.selectedAsteroid == GameLogic.currentIdOfAsteroid) {
            return true;
        }
        return false;
    }

}
