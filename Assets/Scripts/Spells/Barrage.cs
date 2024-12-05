using DG.Tweening;
using Helpers;
using UnityEngine;

namespace Spells
{
    public class Barrage : Spell
    {
        public override void Move(Transform target)
        {
            var moveDuration = Utilities.BASE_MOVE_DURATION / Config.spellSettings.speed;

            transform.DOMove(target.position, moveDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (target.gameObject.TryGetComponent(out Enemy enemy))
                {
                    enemy.TakeDamage(Config.spellSettings.damage);
                    ReturnToPool();
                }
            });
        }
    }
}
