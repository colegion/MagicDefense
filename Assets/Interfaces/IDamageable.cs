using System;
using UnityEngine;

namespace Interfaces
{
    public interface IDamageable
    {
        public abstract void TakeDamage(float amount);

        public abstract void AnimateHit(Action onComplete);

        public abstract void Die();
    }
}
