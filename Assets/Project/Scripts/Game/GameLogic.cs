using System.Collections.Generic;
using MalbersAnimations.Scriptables;
using Project.Scripts.Game.Asteroids;
using Project.Scripts.Game.Buildings;
using Project.Scripts.UI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Game
{
    public class GameLogic : MonoBehaviour
    {
        public static GameLogic Instance;

        public static List<GameObject> bridges = new List<GameObject>();

        public static List<Vector3> bridgePoses1 = new List<Vector3>();
        public static List<Vector3> bridgePoses2 = new List<Vector3>();

        [SerializeField] public List<Sprite> spriteOfMaterials = new List<Sprite>();

        [SerializeField] Text BridgePrice;

        public int selectedAsteroid;

        public int currentIdOfAsteroid;

        public GameObject BridgePrefab;

        [SerializeField] public GameObject victorySign;

        public IntVar enzimaAmount;
        public IntVar chromiumAmount;
        public IntVar linoniumAmount;

        [SerializeField] public GameObject smallExplosion;

        [SerializeField] public int drillEnzimaPrice;
        [SerializeField] public int drillChromiumPrice;
        [SerializeField] public int drillLimoniumPrice;
        [SerializeField] public int turretEnzimaPrice;
        [SerializeField] public int turretChromiumPrice;
        [SerializeField] public int turretLimoniumPrice;
        [SerializeField] public int bridgeLimoniumPrice;

        [SerializeField] private AsteroidsController _asteroidsController;
        [SerializeField] private Builder _builder;
        [SerializeField] private ItemSelector[] _itemSelectors;
        [SerializeField] private BlueprintCreator _blueprintCreator;
        
        public readonly Subject<Asteroid> OnAsteroidEnter = new Subject<Asteroid>();
        public readonly Subject<Asteroid> OnAsteroidDrag = new Subject<Asteroid>();
        public readonly Subject<BuildingData> OnBuildingPick = new Subject<BuildingData>();
        
        private Asteroid _asteroid;

        private void Awake()
        {

            OnAsteroidEnter.TakeUntilDestroy(this).Subscribe();
            _asteroidsController.Construct();

            _blueprintCreator.Construct(OnAsteroidEnter , OnBuildingPick);
            
            _builder.Construct(_itemSelectors , OnBuildingPick, _blueprintCreator);
        }

        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                _builder.Build();
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                enzimaAmount.Value += 200;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                chromiumAmount.Value += 200;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                linoniumAmount.Value += 200;
            }

            BridgePrice.text = bridgeLimoniumPrice.ToString();
        }

        public bool SpendResources(int enzima, int chromium, int linonium)
        {
            if (enzimaAmount - enzima < 0)
            {
                Debug.Log("not enough enzima");
                return false;
            }

            if (chromiumAmount - chromium < 0)
            {
                Debug.Log("not enough chromium");
                return false;
            }

            if (linoniumAmount - linonium < 0)
            {
                Debug.Log("not enough linonium");
                return false;
            }

            enzimaAmount.Value -= enzima;
            chromiumAmount.Value -= chromium;
            linoniumAmount.Value -= linonium;
            return true;
        }
    }
}