using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemyConfig> enemies;
    
    private List<Coroutine> _enemySpawnRoutines = new List<Coroutine>();

    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    private void ConfigureSelf(PoolReadyEvent e)
    {
        InitializeEnemyConfigs();
        StartSpawning();
    }
    
    private void InitializeEnemyConfigs()
    {
        foreach (var config in enemies)
        {
            config.enemySettings.Initialize();
        }
    }

    private void StartSpawning()
    {
        for (int i = 0; i < Enum.GetValues(typeof(EnemyType)).Length; i++)
        {
            var tempRoutine = StartCoroutine(SpawnEnemy((EnemyType)i));
            _enemySpawnRoutines.Add(tempRoutine);
        }
    }
    
    private IEnumerator SpawnEnemy(EnemyType type)
    {
        while (!GameController.Instance.IsGameOver())
        {
            var enemy = GameController.Instance.GetEnemy();
            var config = enemies.Find(e => e.enemyType == type);
            enemy.ConfigureSelf(config);
            enemy.Move(GameController.Instance.GetTowerTransform());
            yield return new WaitForSeconds(config.enemySettings.currentSpawnCooldown);
            
        }
        
        StopSpawning();
    }

    private void StopSpawning()
    {
        foreach (var routine in _enemySpawnRoutines)
        {
            if(routine != null)
                StopCoroutine(routine);
        }
    }

    private void AddListeners()
    {
        EventBus.Instance.Register<PoolReadyEvent>(ConfigureSelf);
    }

    private void RemoveListeners()
    {
        EventBus.Instance.Unregister<PoolReadyEvent>(ConfigureSelf);
    }
}
