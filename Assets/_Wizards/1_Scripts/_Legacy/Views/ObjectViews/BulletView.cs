using System;
using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class BulletView : AmmoView
    {
        protected override void OnResetToPlayer()
        {
            //TODO delete after player get bullet pool
            lifetime /= 3;
        }
        protected override void OnFire(Vector2 direction)
        {
            rigidbody.AddForce(direction * _speed, ForceMode2D.Impulse);
        }

        protected override void OnTriggerExtention(IDamagable target, string tag)
        {
            if (!isFromPlayer && !tag.Equals("Player")) return;
            if (isFromPlayer && tag.Equals("Player")) return;
            target.ReceiveDamage(_damage);
        }
    }
}
