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
    [SerializeField] private MeshRenderer enemyRenderer;
    [SerializeField] private BoxCollider enemyCollider;

    private EnemyConfig _config;
    private float _currentHealth;
    private bool _isActivated;
    private bool _selectedAsTarget;
    private bool _hasEnteredScreen = false;
    
    public static event Action<Enemy> OnEnteredScreen;
    public static event Action<Enemy> OnDie;

    public void ConfigureSelf(EnemyConfig config)
    {
        _config = config;
        _isActivated = true;
        enemyVisual.mesh = _config.enemySettings.enemyMesh;
        enemyRenderer.material = _config.enemySettings.enemyMaterial;
        _currentHealth = _config.enemySettings.health;
        ConfigureScale();
        EnableObject();
    }
    
    private void ConfigureScale()
    {
        transform.localScale = _config.enemySettings.scale;
    }
    
    private void Update()
    {
        if (!_isActivated) return;
        if (IsEnemyOnScreen() && !_hasEnteredScreen)
        {
            _hasEnteredScreen = true;
            OnEnteredScreen?.Invoke(this);
        }
    }

    private bool IsEnemyOnScreen()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPos.x is >= .02f and <= .98f && viewportPos.y is >= .02f and <= .98f;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Tower tower))
        {
            Debug.Log("collision");
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
        if (_currentHealth <= 0) return;
        _currentHealth -= amount;
        AnimateHit(() =>
        {
            if (_currentHealth <= 0)
                Die();
        });
    }

    public void AnimateHit(Action onComplete)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOShakeScale(0.45f, 0.15f).SetEase(Ease.OutCubic));
        var material = enemyRenderer.material;
        sequence.Join(material.DOColor(Color.red, 0.1f).SetEase(Ease.OutCubic));
        sequence.Append(material.DOColor(Color.white, 0.05f).SetEase(Ease.OutCubic));
        sequence.OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public void SetAsSelected(bool value)
    {
        _selectedAsTarget = value;
    }

    public bool GetIsSelected()
    {
        return _selectedAsTarget;
    }

    public bool HasDied()
    {
        return _currentHealth <= 0;
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
        _isActivated = false;
        _currentHealth = 0;
        enemyVisual.mesh = null;
        enemyRenderer.material = null;
        _config = null;
        enemyCollider.size = Vector3.one;
        transform.localScale = Vector3.one;
        OnBecameInvisible();
        gameObject.SetActive(false);
    }
    
    private void OnBecameInvisible()
    {
        _hasEnteredScreen = false;
    }
}
