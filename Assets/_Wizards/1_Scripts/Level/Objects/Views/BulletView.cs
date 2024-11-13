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
            //rigidbody.AddForce(direction * _speed, ForceMode2D.Impulse);
            rigidbody.AddForce(direction * _speed, ForceMode.Impulse);
        }
    }
}
