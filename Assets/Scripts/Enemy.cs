using System.Collections;
using System.Collections.Generic;
using Helpers;
using Interfaces;
using UnityEngine;

public class Enemy : MonoBehaviour, IMovable
{
    [SerializeField] private MeshFilter enemyVisual;
    [SerializeField] private MeshCollider collider;

    private EnemyConfig _config;

    public void ConfigureSelf(EnemyConfig config)
    {
        _config = config;
        enemyVisual.mesh = _config.enemyMesh;
        ConfigureColliderSize();
    }

    private void ConfigureColliderSize()
    {
        
    }
    
    public void Move(Transform target)
    {
        throw new System.NotImplementedException();
    }
}
