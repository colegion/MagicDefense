using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(menuName = "Spell Settings", fileName = "New Spell Config")]
    public class SpellSettings : ScriptableObject
    {
        public float speed;
        public float cooldown;
        public float damage;
    }
}
