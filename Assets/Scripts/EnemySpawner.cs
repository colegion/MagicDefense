using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
using Random = UnityEngine.Random;

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
            Vector3 spawnPosition = GetRandomSpawnPosition();
            enemy.transform.position = spawnPosition;
            enemy.ConfigureSelf(config);
            enemy.Move(GameController.Instance.GetTowerTransform());
            yield return new WaitForSeconds(config.enemySettings.currentSpawnCooldown);
        }
        
        StopSpawning();
    }
    
    private Vector3 GetRandomSpawnPosition()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
            throw new Exception("Main Camera not found!");
        
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        float screenLeft = bottomLeft.x;
        float screenRight = topRight.x;
        float screenBottom = bottomLeft.z;
        float screenTop = topRight.z;
        
        float offset = 2f;

        // Randomly choose an edge: 0 = left, 1 = right, 2 = top, 3 = bottom
        int edge = Random.Range(0, 4);

        switch (edge)
        {
            case 0: // Left
                return new Vector3(screenLeft - offset, 0, Random.Range(screenBottom, screenTop));
            case 1: // Right
                return new Vector3(screenRight + offset, 0, Random.Range(screenBottom, screenTop));
            case 2: // Top
                return new Vector3(Random.Range(screenLeft, screenRight), 0, screenTop + offset);
            case 3: // Bottom
                return new Vector3(Random.Range(screenLeft, screenRight), 0, screenBottom - offset);
            default:
                throw new Exception("Invalid edge case.");
        }
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
        if(EventBus.Instance != null)
            EventBus.Instance.Unregister<PoolReadyEvent>(ConfigureSelf);
    }
}
