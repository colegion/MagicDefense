using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Helpers;
using Interfaces;
using Scriptables;
using Unity.VisualScripting;
using UnityEngine;
using IPoolable = Interfaces.IPoolable;

public class Spell : MonoBehaviour, IPoolable, IMovable
{
    [SerializeField] private MeshFilter spellMesh;
    protected SpellConfig Config;
    
    public void ConfigureSelf(SpellConfig config)
    {
        Config = config;
        spellMesh.mesh = Config.spellMesh;
    }
    
    public virtual void Move(Transform target)
    {
        var duration = Utilities.BASE_MOVE_DURATION / Config.spellSettings.speed;
        transform.DOMove(target.position, duration).SetEase(Ease.Linear);
    }

    public void EnableObject()
    {
        throw new NotImplementedException();
    }

    public void ReturnToPool()
    {
        Config = null;
        spellMesh.mesh = null;
    }
}
