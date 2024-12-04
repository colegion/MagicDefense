using UnityEngine;

namespace Interfaces
{
    public interface IDamageable
    {
        public abstract void TakeDamage(float amount);

        public abstract void Die();
    }
}
