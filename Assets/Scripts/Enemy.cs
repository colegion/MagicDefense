using System.Collections;
using System.Collections.Generic;
using Helpers;
using Interfaces;
using UnityEngine;

public class Enemy : MonoBehaviour, IMovable, IDamageable, IPoolable
{
    [SerializeField] private MeshFilter enemyVisual;
    [SerializeField] private MeshCollider collider;

    private EnemyConfig _config;
    private float _currentHealth;

    public void ConfigureSelf(EnemyConfig config)
    {
        _config = config;
        enemyVisual.mesh = _config.enemyMesh;
        _currentHealth = _config.enemySettings.health;
        ConfigureColliderSize();
    }

    private void ConfigureColliderSize()
    {
        
    }
    
    public void Move(Transform target)
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        if (_currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        ResetSelf();
    }

    public void ResetSelf()
    {
        _currentHealth = 0;
        enemyVisual.mesh = null;
        _config = null;
    }
}
