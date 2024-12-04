using System.Collections;
using System.Collections.Generic;
using Helpers;
using Interfaces;
using Scriptables;
using Unity.VisualScripting;
using UnityEngine;
using IPoolable = Interfaces.IPoolable;

public class Spell : MonoBehaviour, IPoolable, IMovable
{
    [SerializeField] private MeshFilter spellMesh;
    private SpellConfig _config;
    
    public void ConfigureSelf(SpellConfig config)
    {
        _config = config;
        spellMesh.mesh = _config.spellMesh;
    }
    
    public void Move(Transform target)
    {
        throw new System.NotImplementedException();
    }
    
    public void ResetSelf()
    {
        throw new System.NotImplementedException();
    }

    
}
