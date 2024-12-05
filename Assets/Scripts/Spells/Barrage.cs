using System.Collections;
using DG.Tweening;
using Helpers;
using UnityEngine;

namespace Spells
{
    public class Barrage : Spell
    {
        private Coroutine _followTargetRoutine;

        public override void Move(Transform target)
        {
            target = GameController.Instance.GetAvailableEnemy()?.transform;
            if (target == null)
            {
                Debug.LogWarning("No available enemy to target!");
                ReturnToPool();
                return;
            }
            
            if (_followTargetRoutine != null)
            {
                StopCoroutine(_followTargetRoutine);
            }

            _followTargetRoutine = StartCoroutine(FollowTarget(target));
        }

        private IEnumerator FollowTarget(Transform target)
        {
            while (target != null && Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target.position,
                    Time.deltaTime * Config.spellSettings.speed
                );

                yield return null;
            }
            
            OnTargetReached(target);
        }

        private void OnTargetReached(Transform target)
        {
            if (target != null && target.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(Config.spellSettings.damage);
            }

            ReturnToPool();
        }

        public override void ReturnToPool()
        {
            if (_followTargetRoutine != null)
            {
                StopCoroutine(_followTargetRoutine);
                _followTargetRoutine = null;
            }

            base.ReturnToPool();
        }
    }
}
