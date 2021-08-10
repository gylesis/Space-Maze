using Project.Scripts.UI;
using UnityEngine;

namespace Project.Scripts.Game.Buildings
{
    public class BridgeVisual : MonoBehaviour
    {
        /*private LineRenderer _bridgeLine;
        private static GameLogic _gameLogic;

        private void Awake()
        {
            _gameLogic = FindObjectOfType<GameLogic>();
            _bridgeLine = GetComponent<LineRenderer>();
        }

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

            var bridge = Instantiate(GameLogic.Instance.BridgePrefab);

            var startPoint = _gameLogic.Asteroids[_gameLogic.currentIdOfAsteroid].transform;
            var targetPoint = _gameLogic.Asteroids[_gameLogic.selectedAsteroid].transform;

            BridgeRender bridgeRenderer = bridge.GetComponent<BridgeRender>();
            bridgeRenderer.SetPoses(startPoint.position, targetPoint.position);

            GameLogic.bridges.Add(bridge);
            DefineBridgePoses();

            EdgeCollider2D collider = bridge.AddComponent<EdgeCollider2D>();
            collider.isTrigger = true;
            Vector2[] newVerts =
            {
                bridgeRenderer.pos1,
                bridgeRenderer.pos2
            };
            collider.points = newVerts;

            if (_gameLogic.selectedAsteroid == _gameLogic.Asteroids.Count - 2)
            {
                VictoryCondition.win = true;
                Debug.Log("You Won");
                CreateFinalBridge();
            }
        }

        public static void CreateFinalBridge()
        {
            var bridge = Instantiate(GameLogic.Instance.BridgePrefab);

            var startPoint = _gameLogic.Asteroids[_gameLogic.selectedAsteroid].transform;
            var targetPoint = _gameLogic.Asteroids[_gameLogic.Asteroids.Count - 1].transform;

            BridgeRender bridgeRenderer = bridge.GetComponent<BridgeRender>();
            bridgeRenderer.SetPoses(startPoint.position, targetPoint.position);

            GameLogic.bridges.Add(bridge);
            DefineBridgePoses();

            EdgeCollider2D collider = bridge.AddComponent<EdgeCollider2D>();
            collider.isTrigger = true;
            Vector2[] newVerts =
            {
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
           // ограничение строительства по длине
        }

        private void OnMouseEnter()
        {
            if (CheckForAllowToBuild() && !GameLogic.inMenu)
            {
                if (gameObject.name == "Castle")
                {
                    print("sdsadasd");
                    _bridgeLine.SetPosition(0, _gameLogic.Asteroids[_gameLogic.currentIdOfAsteroid].transform.position);
                    _bridgeLine.SetPosition(1, transform.position);
                }
                else
                {
                    _bridgeLine.SetPosition(0, _gameLogic.Asteroids[_gameLogic.currentIdOfAsteroid].transform.position);
                    _bridgeLine.SetPosition(1, _gameLogic.Asteroids[_gameLogic.selectedAsteroid].transform.position);
                }
            }
        }

        private void OnMouseExit()
        {
            _bridgeLine.SetPosition(0, _gameLogic.Asteroids[_gameLogic.currentIdOfAsteroid].transform.position);
            _bridgeLine.SetPosition(1, _gameLogic.Asteroids[_gameLogic.currentIdOfAsteroid].transform.position);
        }*/
    }
}