using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Helpers;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

public class Tower : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject towerHead;
    [SerializeField] private GameObject towerBody;
    [SerializeField] private MeshRenderer bodyRenderer;
    [SerializeField] private List<SpellConfig> spells;
    [SerializeField] private float health;
    private List<Coroutine> _spellRoutines;

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
        while (GameController.Instance.GetOnScreenEnemies().Count == 0)
            yield return null;
        
        var spawnCount = type == SpellType.Fireball ? 1 : GameController.Instance.GetOnScreenEnemies().Count;
        var config = spells.Find(s => s.spellType == type);
        while (!GameController.Instance.IsGameOver())
        {
            for (int i = 0; i < spawnCount; i++)
            {
                Debug.Log("spawn count :" + spawnCount);
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
        if (health <= 0) return;
        health -= amount;
        AnimateHit(() =>
        {
            if (health <= 0)
            {
                StopCoroutines();
                Die();
                GameController.Instance.SetGameOver(true);
            }
        });
    }

    public void AnimateHit(Action onComplete)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(towerBody.transform.DOShakeScale(0.3f, 0.5f).SetEase(Ease.OutBounce));
        var material = bodyRenderer.material;
        sequence.Join(material.DOColor(Color.red, 0.3f).SetEase(Ease.Flash));
        sequence.Append(towerHead.transform.DOJump(transform.localPosition + Vector3.up, .2f, 1, 0.3f));
        sequence.Join(material.DOColor(Color.white, 0.2f).SetEase(Ease.Flash));
        sequence.OnComplete(() =>
        {
            onComplete?.Invoke();
        });
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
        Debug.Log("Died");
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
