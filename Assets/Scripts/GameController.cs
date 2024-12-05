using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using Interfaces;
using Pool;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform towerTransform;
    [SerializeField] private PoolController poolController;

    private static GameController _instance;
    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameController>();
            }

            return _instance;
        }
    }

    private List<Enemy> _onScreenEnemies = new List<Enemy>();
    private bool _gameOver;

    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }
    
    public Enemy GetEnemy()
    {
        return poolController.GetEnemy();
    }
    
    public Spell GetSpell(SpellType type)
    {
        return poolController.GetSpell(type);
    }

    public List<Enemy> GetOnScreenEnemies()
    {
        return _onScreenEnemies;
    }

    public Enemy GetAvailableEnemy()
    {
        foreach (var enemy in _onScreenEnemies)
        {
            if (!enemy.GetIsSelected())
            {
                enemy.SetAsSelected(true);
                return enemy;
            }
        }
        return null;
    }
    
    public void ResetEnemySelectedStatus()
    {
        foreach (var enemy in _onScreenEnemies)
        {
            enemy.SetAsSelected(false);
        }
    }

    private void OnEnemyEnteredScreen(Enemy enemy)
    {
        _onScreenEnemies.Add(enemy);
    }

    private void OnEnemyDie(Enemy enemy)
    {
        _onScreenEnemies.Remove(enemy);
    }

    public Transform GetTowerTransform()
    {
        return towerTransform;
    }
    
    public void SetGameOver(bool value)
    {
        _gameOver = value;
    }

    public bool IsGameOver()
    {
        return _gameOver;
    }

    private void AddListeners()
    {
        Enemy.OnEnteredScreen += OnEnemyEnteredScreen;
        Enemy.OnDie += OnEnemyDie;
    }

    private void RemoveListeners()
    {
        Enemy.OnEnteredScreen -= OnEnemyEnteredScreen;
        Enemy.OnDie -= OnEnemyDie;
    }
}
