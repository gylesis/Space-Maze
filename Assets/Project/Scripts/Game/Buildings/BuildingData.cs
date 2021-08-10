using Project.Scripts.Game.Asteroids;
using UniRx;
using UnityEngine;

namespace Project.Scripts.Game.Buildings
{
    [CreateAssetMenu(menuName = "Data/BuildingData", fileName = "BuildingData", order = 0)]
    public class BuildingData : ScriptableObject
    {
        [SerializeField] private int _enzimaPrice;
        [SerializeField] private int _chromiumPrice;
        [SerializeField] private int _linoniumPrice;

        public GameObject Blueprint;
        public GameObject ExplosionPrefab;
        public Sprite Icon;
        public Building Prefab;
        
        public float Health = 100;
        public float MaxHealth = 100;

        public Subject<float> OnHeatlhChanged = new Subject<float>();
        
        public Asteroid LocatingAsteroid { get; set; }

        public void TakeDamage(float damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                
            }
            
            OnHeatlhChanged.OnNext(Health);
            
        }
       
    }
}