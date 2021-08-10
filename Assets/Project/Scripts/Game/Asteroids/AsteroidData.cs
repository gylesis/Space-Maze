using MalbersAnimations.Scriptables;
using UniRx;
using UnityEngine;

namespace Project.Scripts.Game.Asteroids
{
    [CreateAssetMenu(menuName = "Data/AsteroidData", fileName = "AsteroidData", order = 0)]
    public class AsteroidData : ScriptableObject
    {
        [SerializeField] private Sprite _spriteOfCurrentMaterial;
        
        private float _oreAmount;
        
        public Asteroid.TypeOfAsteroid TypeOfAsteroid;

        public readonly Subject<Unit> OnOreLow = new Subject<Unit>();
        public readonly Subject<float> OnOreEvaluate = new Subject<float>();
        
        
        public void Mine(float value)
        {
            _oreAmount -= value;
            
            OnOreEvaluate.OnNext(_oreAmount);
            
            if (_oreAmount <= 0) 
                OnOreLow.OnNext(Unit.Default);
        }
        
        public void Construct()
        {
            SetOreAmount();
        }
                
        private void SetOreAmount() =>
            _oreAmount = Random.value * 100 + 200;
        
    }
}