using Project.Scripts.Game.Asteroids;
using UniRx;
using UnityEngine;

namespace Project.Scripts.Game.Buildings
{
    public class Building : MonoBehaviour
    {
        [SerializeField] protected BuildingData _buildingData;
        protected float Health => _buildingData.Health;
        protected float MaxHealth => _buildingData.MaxHealth;
        protected float RemainingHealth => Health - MaxHealth;
    
        private static readonly int Leave = Animator.StringToHash("Leave");

        protected int idOfCurrentAsteroid { get; set; }

        public void Construct(Asteroid buildDataLocationAsteroid)
        {
            buildDataLocationAsteroid.OnDestroy
                .Take(1)
                .Subscribe(_ => OnDeath());
        }
        
        public void TakeDamage(int damage)  // НЛО при убийстве здания улетало TODO
        {
            var health = Health;
        
            health -= damage;
        
            if (health <= 0) 
                OnDeath();
        }

        public void OnDeath()
        {
            Destroy(Instantiate(_buildingData.ExplosionPrefab, transform.position, Quaternion.identity), 2f);
            Destroy(gameObject);
        }
    }
}