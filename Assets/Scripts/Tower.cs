using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

public class Tower : MonoBehaviour, IDamageable
{
    [SerializeField] private List<SpellConfig> spells;
    [SerializeField] private float health;
    private bool _isGameOver;
    private List<Coroutine> _spellRoutines;
    
    public void ConfigureSelf()
    {
        _spellRoutines = new List<Coroutine>();
        StartSpellFirings();
    }

    private void StartSpellFirings()
    {
        for (int i = 0; i < Enum.GetValues(typeof(SpellType)).Length; i++)
        {
            Coroutine tempRoutine = StartCoroutine(CastSpell((SpellType)i));
            _spellRoutines.Add(tempRoutine);
        }
    }

    private IEnumerator CastSpell(SpellType type)
    {
        var spawnCount = type == SpellType.Fireball ? 1 : GameController.Instance.GetOnScreenEnemies().Count;
        var config = spells.Find(s => s.spellType == type);
        while (!_isGameOver)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                var spell = GameController.Instance.GetSpell(type);
                spell.ConfigureSelf(config);
                spell.Move(null);
            }

            yield return new WaitForSeconds(config.spellSettings.cooldown);
            GameController.Instance.ResetEnemySelectedStatus();
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            StopCoroutines();
            Die();
            _isGameOver = true;
        }
    }

    private void StopCoroutines()
    {
        foreach (var routine in _spellRoutines)
        {
            if(routine != null)
                StopCoroutine(routine);
        }
    }

    public void Die()
    {
        
    }
}
