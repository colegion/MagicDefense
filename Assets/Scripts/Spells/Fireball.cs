using System;
using Helpers;
using UnityEngine;

namespace Spells
{
    public class Fireball : Spell
    {
        public override void Move(Transform target)
        {
            var moveDuration = Utilities.BASE_MOVE_DURATION / Config.spellSettings.speed;
        }

        private void OnCollisionEnter(Collision other)
        {
            throw new NotImplementedException();
        }
    }
}
