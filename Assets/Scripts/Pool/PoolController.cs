using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Pool
{
    public class PoolController : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject spellPrefab;

        [SerializeField] private int poolSize;

        private EnemyPool _enemyPool;
        private SpellPool _spellPool;
        
        private List<IPoolable> _activeObjects = new List<IPoolable>();

        private void Start()
        {
            var poolTransform = transform;
            _enemyPool = new EnemyPool(enemyPrefab, poolSize, poolTransform);
            _spellPool = new SpellPool(spellPrefab, poolSize, poolTransform);
        }

        public void AppendActiveObjects(IPoolable poolable)
        {
            _activeObjects.Add(poolable);
        }

        public Enemy GetEnemy()
        {
            return _enemyPool.GetObject();
        }

        private void ReturnEnemy(Enemy enemy)
        {
            _enemyPool.ReturnObject(enemy);
        }

        public Spell GetSpell()
        {
            return _spellPool.GetObject();
        }

        private void ReturnSpell(Spell spell)
        {
            _spellPool.ReturnObject(spell);
        }
        
        public void ClearActiveObjects()
        {
            foreach (var obj in _activeObjects)
            {
                if (obj is Enemy enemy)
                {
                    ReturnEnemy(enemy);
                }
                else if (obj is Spell spell)
                {
                    ReturnSpell(spell);
                }
            }
            _activeObjects.Clear();
        }
    }
}
