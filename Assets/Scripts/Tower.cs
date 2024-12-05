using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

public class Tower : MonoBehaviour, IDamageable
{
    [SerializeField] private float health;
    private bool _isGameOver;
    private List<Coroutine> _spellRoutines;
    
    public static event Action OnGameOver;
    public void ConfigureSelf()
    {
        _spellRoutines = new List<Coroutine>();
        StartSpellFirings();
    }

    private void StartSpellFirings()
    {
        for (int i = 0; i < Enum.GetValues(typeof(SpellType)).Length; i++)
        {
            Coroutine tempRoutine =
                StartCoroutine((SpellType)i == SpellType.Fireball ? CastFireBall() : CastBarrage());
            _spellRoutines.Add(tempRoutine);
        }
    }

    private IEnumerator CastSpell(SpellType type)
    {
        while (!_isGameOver)
        {
            
        }
        
        yield return null;
    }

    private IEnumerator CastFireBall()
    {
        while (!_isGameOver)
        {
            //@todo: pool system and casting spells.
            yield return new WaitForSeconds(1f);
        }
    }
    
    private IEnumerator CastBarrage()
    {
        while (!_isGameOver)
        {
            yield return new WaitForSeconds(1f);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0) Die();
    }

    public void Die()
    {
        OnGameOver?.Invoke();
    }
}
