using UnityEngine;

namespace Pool
{
    public class SpellPool : ObjectPool<Spell>
    {
        public SpellPool(GameObject prefab, int initialSize, Transform parent = null) : base(prefab, initialSize, parent)
        {
        }
    }
}
