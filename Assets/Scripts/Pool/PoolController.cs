using System.Collections.Generic;
using Helpers;
using Interfaces;
using UnityEngine;

namespace Pool
{
    public class PoolController : MonoBehaviour
    {
        [SerializeField] private Transform towerTransform;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject fireBallPrefab;
        [SerializeField] private GameObject barragePrefab;

        [SerializeField] private int poolSize;

        private EnemyPool _enemyPool;
        private Dictionary<SpellType, SpellPool> _spellPools;
        private List<IPoolable> _activeObjects = new List<IPoolable>();

        private void Start()
        {
            var poolTransform = transform;
            _enemyPool = new EnemyPool(enemyPrefab, poolSize, poolTransform);
            _spellPools = new Dictionary<SpellType, SpellPool>
            {
                { SpellType.Fireball, new SpellPool(fireBallPrefab, poolSize, towerTransform) },
                { SpellType.Barrage, new SpellPool(barragePrefab, poolSize, towerTransform) }
            };
            
            EventBus.Instance.Trigger(new PoolReadyEvent());
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

        public Spell GetSpell(SpellType type)
        {
            return _spellPools[type].GetObject();
        }

        private void ReturnSpell(SpellType type, Spell spell)
        {
            _spellPools[type].ReturnObject(spell);
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
                    ReturnSpell(spell.GetSpellType(), spell);
                }
            }
            _activeObjects.Clear();
        }
    }
}
