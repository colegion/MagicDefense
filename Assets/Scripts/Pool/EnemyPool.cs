using UnityEngine;

namespace Pool
{
    public class EnemyPool : ObjectPool<Enemy>
    {
        public EnemyPool(GameObject prefab, int initialSize, Transform parent = null) : base(prefab, initialSize, parent)
        {
            Initialize();
        }
    }
}
