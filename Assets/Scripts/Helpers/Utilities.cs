using System;
using System.Collections.Generic;
using Scriptables;
using UnityEngine;

namespace Helpers
{
    public class Utilities : MonoBehaviour
    {
        public static float BASE_MOVE_DURATION = 10f;
    }

    [Serializable]
    public class EnemyConfig
    {
        public EnemyType enemyType;
        public EnemySettings enemySettings;
    }

    [Serializable]
    public class SpellConfig
    {
        public SpellType spellType;
        public SpellSettings spellSettings;
    }

    public enum EnemyType
    {
        Base,
        Speedy,
        Bulky
    }

    public enum SpellType
    {
        Fireball,
        Barrage
    }
    
    public class PoolReadyEvent
    {
            
    }
}
