using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Game.Buildings
{
    public class MiningDrill : Building
    {
        [SerializeField] float tickTime = 1f;

        private int materialPerTick = 3;
        private bool _allowToTick = true;
        private GameLogic _gameLogic;
    
        private static readonly int Setup = Animator.StringToHash("Setup");

        private void Awake()
        {
            _gameLogic = FindObjectOfType<GameLogic>();
        
            GetComponent<Animator>().SetTrigger(Setup);

            var slider = GetComponentInChildren<Slider>();
            var healthBar = new HealthBar(slider);

            _buildingData.OnHeatlhChanged.Subscribe(healthBar.UpdateBar);
        }

        protected void Update()
        {
            if (_allowToTick)
            {
                _allowToTick = false;
                StartCoroutine(Income());
            }
        }

        IEnumerator Income()
        {
            switch (idOfCurrentAsteroid)
            {
                case 1:
                    _gameLogic.enzimaAmount.Value += materialPerTick;
                    break;

                case 2:
                    _gameLogic.chromiumAmount.Value += materialPerTick;
                    break;

                case 3:
                    _gameLogic.linoniumAmount.Value += materialPerTick;
                    break;
            }

            //LocatingAsteroid.oreAmount -= materialPerTick;

            yield return new WaitForSeconds(tickTime);
            _allowToTick = true;
        }
    }
}