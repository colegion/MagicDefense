using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class Tower : MonoBehaviour, IDamageable
{
    private float _health;

    public static event Action OnGameOver;
    public void ConfigureSelf()
    {
        
    }

    public void TakeDamage(float amount)
    {
        _health -= amount;
        if(_health <= 0) Die();
    }

    public void Die()
    {
        OnGameOver?.Invoke();
    }
}
