using UnityEngine;
using UnityEngine.Serialization;

namespace Scriptables
{
    [CreateAssetMenu(menuName = "Enemy Settings", fileName = "New Enemy Config")]
    public class EnemySettings : ScriptableObject
    {
        public float speed;
        public float health;
        public int damage;
        public float defaultSpawnCooldown;
        public float currentSpawnCooldown;
        public Vector3 scale;
        public Vector3 colliderSize;

        public void Initialize()
        {
            currentSpawnCooldown = defaultSpawnCooldown;
        }
        
        public void ReduceSpawnCooldown(float amount)
        {
            currentSpawnCooldown -= amount;
        }
    }
}
