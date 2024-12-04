using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(menuName = "Enemy Settings", fileName = "New Enemy Config")]
    public class EnemySettings : ScriptableObject
    {
        public float speed;
        public float health;
        public int damage;
    }
}
