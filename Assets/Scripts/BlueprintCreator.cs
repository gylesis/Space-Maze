using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintCreator : MonoBehaviour {

    public static BlueprintCreator Instance;

    [SerializeField]
    List<GameObject> blueprints = new List<GameObject>();

    [SerializeField]
    public GameObject blueprintToView;

    public static Vector3 posToBuild;

    public static Quaternion rotationToBuild;

    private void Start() {
        Instance = this;
    }
    private void Update() {
        Vector3 mousePos = MousePosition.GetMouseWorldPosition();
        Vector3 centerOfAsteroid = GameLogic.Asteroids[GameLogic.selectedAsteroid].transform.position;


        Vector3 direction = (mousePos - centerOfAsteroid).normalized;

        RaycastHit2D rayToSurface = Physics2D.Raycast(centerOfAsteroid + direction * 3f, -direction);

        posToBuild = rayToSurface.point;

        blueprintToView.transform.position = posToBuild;
        rotationToBuild = Quaternion.FromToRotation(Vector3.up, rayToSurface.normal);

        blueprintToView.transform.rotation = rotationToBuild;
    }

    public void ShowBlueprint() {
        if (!Builder.Instance.CheckForAllowToBuild()) {
            return;
        }
        else if (!GameLogic.isStateToCreate) {
            return;
        }

        blueprintToView = blueprints[GameLogic.currentBuildingToBuild];
        blueprintToView = Instantiate(blueprintToView);
    }

    public void DeleteBluepint() {
        if (!Builder.Instance.CheckForAllowToBuild()) {
            return;
        }
        else if (!GameLogic.isStateToCreate) {
            return;
        }
        Destroy(blueprintToView);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(posToBuild, 1);


    }




}
