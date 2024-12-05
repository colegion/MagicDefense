using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Helpers;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour, IMovable, IDamageable, IPoolable
{
    [SerializeField] private MeshFilter enemyVisual;
    [SerializeField] private MeshCollider enemyCollider;

    private EnemyConfig _config;
    private float _currentHealth;
    private bool _selectedAsTarget;

    public static event Action<Enemy> OnEnteredScreen;
    public static event Action<Enemy> OnDie;

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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Tower tower))
        {
            tower.TakeDamage(_config.enemySettings.damage);
            Die();
        }
    }

    public void Move(Transform target)
    {
        var moveDuration = Utilities.BASE_MOVE_DURATION / _config.enemySettings.speed;
        transform.DOMove(target.position, moveDuration).SetEase(Ease.Linear);
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        if (_currentHealth <= 0)
            Die();
    }

    public void SetAsSelected(bool value)
    {
        _selectedAsTarget = value;
    }

    public bool GetIsSelected()
    {
        return _selectedAsTarget;
    }

    public void Die()
    {
        OnDie?.Invoke(this);
        ReturnToPool();
    }

    public void EnableObject()
    {
        gameObject.SetActive(true);
    }

    public void ReturnToPool()
    {
        _currentHealth = 0;
        enemyVisual.mesh = null;
        _config = null;
        gameObject.SetActive(false);
    }
}
