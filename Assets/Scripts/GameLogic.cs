using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

    public static GameLogic Instance;

    [SerializeField]
    public static List<GameObject> Asteroids = new List<GameObject>();

    public static List<GameObject> bridges = new List<GameObject>();

    public static List<Vector3> bridgePoses1 = new List<Vector3>();
    public static List<Vector3> bridgePoses2 = new List<Vector3>();

    [SerializeField]
    public List<Sprite> spriteOfMaterials = new List<Sprite>();

    [SerializeField]
    Text BridgePrice;

    public static int asteroidsCounter = 0;

    public static int selectedAsteroid = 0;

    public static int currentIdOfAsteroid = 0;

    public static int currentBuildingToBuild = 0;

    public static bool inMenu = false;

    public GameObject bridgePrefab;

    [SerializeField]
    public GameObject victorySign;

    public static bool isStateToCreate = false;

    public static int enzimaAmount = 0;
    public static int chromiumAmount = 0;
    public static int linoniumAmount = 0;

    [SerializeField]
    public GameObject explosionPrefab;
    [SerializeField]
    public GameObject smallExplosion;

    [SerializeField]
    public int drillEnzimaPrice;
    [SerializeField]
    public int drillChromiumPrice;
    [SerializeField]
    public int drillLimoniumPrice;
    [SerializeField]
    public int turretEnzimaPrice;
    [SerializeField]
    public int turretChromiumPrice;
    [SerializeField]
    public int turretLimoniumPrice;

    [SerializeField]
    public int bridgeLimoniumPrice;

    private void Start() {
        Instance = this;
        AsteroidsSort();

        enzimaAmount = 100;
        chromiumAmount = 100;
        linoniumAmount = 100;

    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Builder.Instance.Build();
        }
        if (Input.GetMouseButtonDown(1) && !inMenu) {
            isStateToCreate = false;
            Destroy(BlueprintCreator.Instance.blueprintToView);
        }

        if (Input.GetKeyDown(KeyCode.Z)) {
            enzimaAmount += 200;
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            chromiumAmount += 200;
        }
        if (Input.GetKeyDown(KeyCode.C)) {
            linoniumAmount += 200;
        }


        BridgePrice.text = bridgeLimoniumPrice.ToString();
    }

    private void AsteroidsSort() {

        for (int j = 0; j < Asteroids.Count - 1; j++) {
            for (int i = 0; i < Asteroids.Count - 1; i++) {
                if (Asteroids[i].transform.position.x > Asteroids[i + 1].transform.position.x) {
                    var temp = Asteroids[i];
                    Asteroids[i] = Asteroids[i + 1];
                    Asteroids[i + 1] = temp;
                }
            }
        }
    }

    public static bool SpendResources(int enzima,int chromium,int linonium) {
        if(enzimaAmount - enzima < 0) {
            Debug.Log("not enough enzima");
            return false;
        }
        else if(chromiumAmount - chromium < 0) {
            Debug.Log("not enough chromium");
            return false;
        }
        else if(linoniumAmount - linonium < 0) {
            Debug.Log("not enough linonium");
            return false;
        }

        enzimaAmount -= enzima;
        chromiumAmount -= chromium;
        linoniumAmount -= linonium;
        return true;

    }


}
